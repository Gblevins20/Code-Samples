/* Simple process scheduler program */

/* include c header files */
#include <stdlib.h>
#include <unistd.h> // for function fork()
#include <stdio.h>
#include <string>
#include <iostream>
#include <iomanip>
#include <fstream>
#include <sstream>
#include <cstring>
#include <signal.h>
#include <sys/stat.h>
#include <fcntl.h>
#include <time.h>
#include <algorithm>

#define N 100 
#define MAX_QUEUE 100 //Maximum possible number of jobs in a queue is 100. It is unlikely, but not impossible
#define SERVER_QUEUE 0
#define POWER_USER_QUEUE 1 //All three queues are defined by a particular number
#define USER_QUEUE 2

using namespace std;

void setJobQueues();
void jobGenerator();
void jobScheduler();
int selectJob();
void executeJob(int* jobID);


void down(int *semid, char* semname); //Provided by instructor for mutex use
void up(int semid, char* semname);
void wake_up(int);

char filename[] = "queuefile.txt"; //Used to keep the exact same file name throughout
char semname[10]; //Used for mutex

int intr = 0; //Used for mutex
int semid = 0;

int main() {


	int pid = 0;

	strcpy(semname, "mutex"); //Used for mutex

	setJobQueues();  /* Set up the priority job queues with chosen file and/or data structure */
	if (pid = fork() > 0) { /* jobGenerator process */
		down(&semid, semname);
		jobGenerator(); /* generate random jobs and put them into the priority queues. The priority queues must be protected in a critical region */
		up(semid, semname);
		usleep(1000); //Use sleep() function to synchronize the job generation and job scheduling processes. 
		exit(0);
	}
	else {/* [job scheduler process] */
		down(&semid, semname);
		jobScheduler();  /* schedule and execute the jobs. */
		up(semid, semname);
		remove(filename); //Clears files
		remove(semname);
		exit(0);
	}

	system("pause");
	return (1);
}

void setJobQueues()
{
	fstream file;

	file.open(filename, fstream::in | fstream::out | fstream::trunc); //Sets up file for future use

	if (!file) {
		cout << "File Not Found" << endl;
	}

	for (int i = 0; i < 3; i++) { //Set up queues in file
		file << endl;
	}

	file.close();
}

void jobGenerator() //You may simulate by generating 100 jobs
{
	int jobsProduced = 0, jobNum = 0, quantumTimeInput = 0, jobPriority = 0; //All variables necessary for a job
	
	//Generates job and priority
	while (jobsProduced < N) {
		srand(time(0) + jobsProduced); //Randomizes jobNumber
		jobNum = rand() % 99 + 1;


		stringstream jobNumInput;
		jobNumInput << setw(2) << setfill('0') << jobNum;

		//Generates Quantum Time for a job
		quantumTimeInput = rand() % 3 + 1;
		stringstream quantumStreamInput;
		quantumStreamInput << setw(2) << setfill('0') << quantumTimeInput;

		string jobInfo = jobNumInput.str() + quantumStreamInput.str(); //Collects the job information from the previous creation for the future

		//Sets job priority
		if (!(jobNum < 1) && jobNum <= 30) {
			jobPriority = SERVER_QUEUE;
		}
		else if (jobNum <= 60) {
			jobPriority = POWER_USER_QUEUE;
		}
		else if (jobNum <= 100) {
			jobPriority = USER_QUEUE;
		}
		else {
			cout << "Out of Range Exception" << endl;
		}

		//Critical Region Start
		ifstream criticalRegionInput;
		criticalRegionInput.open(filename);

		string fileContents = "", lineContent;
		while (getline(criticalRegionInput, lineContent)) {
			fileContents += lineContent + "\n";
		}

		criticalRegionInput.close();
		//Critical Region End

		//Find Start position matching priority
		int start = 0, end = 0;
		for (int i = 0; i < jobPriority; i++) {
			start = fileContents.find_first_of('\n', start) + 1;
		}
		end = fileContents.find_first_of('\n', start);

		//Checks how long the queue is
		string priorityLineLength = fileContents.substr(start, end - start);

		int entries = 0, values = 0;

		for (int i = 0; i < priorityLineLength.length(); i++) {
			if (priorityLineLength[i] != ' ' || priorityLineLength[i] != '\n') {
				values++;
			}
		}

		entries = values / 4;

		if (entries < MAX_QUEUE) { //As long as the queue is not too large already, it will insert the job into the queue
			if (entries == 0) {
				fileContents.insert(end, jobInfo);
			}
			else
			{
				jobInfo.insert(0, 1, ' ');
				fileContents.insert(end, jobInfo);
			}


			//Critical Region Start
			ofstream fileOutput;
			fileOutput.open(filename);
			fileOutput.write(fileContents.c_str(), fileContents.length());
			fileOutput.close();
			//Crit Region End

			//QueueName Check
			string queueName = "";
			switch (jobPriority) {
			case SERVER_QUEUE:
				queueName = "SERVER_QUEUE";
				break;
			case POWER_USER_QUEUE:
				queueName = "POWER_USER_QUEUE";
				break;
			case USER_QUEUE:
				queueName = "USER_QUEUE";
				break;
			}

			//Check Generated Job
			cout << "Generated Job " << jobInfo << " of queue " << queueName << endl;
		}
		else
		{
			cout << "Queue Full, no new job added." << endl;
		}

		jobsProduced++;
		sleep(1);
	}
}

void jobScheduler() {
	int i = 0, n = 0, pid = 0;
	while (i < N) {	/* schedule and run maximum N jobs */
		n = selectJob();  /* pick a job from the job priority queues */
		if (n > 0) { /* valid job id */
			if (pid = fork() == 0) { /* child worker process */
				executeJob(&n);	 /* execute the job */
				exit(0);
			}
		}
		i++;
	}
}

int selectJob()
{
	//Critical Region Start
	fstream fileInput;
	ofstream fileOutput;
	fileInput.open(filename);

	string currentQueue = "";
	int jobIndex = 0;
	for (int i = 0; i < 3; i++) { //Loads up every queue
		getline(fileInput, currentQueue);
		if (jobIndex = currentQueue.find_first_not_of('\n') != string::npos) { //Checks every job to find the most recent on the queue
			jobIndex--;
			string jobNum = currentQueue.substr(jobIndex, 4); //Loads up that job's information

			string fileContents = "", lineContents = "";
			fileInput.clear();
			fileInput.seekg(ifstream::beg);
			while (getline(fileInput, lineContents)) { //Stores the job from a queue to a temp holder
				fileContents += lineContents + "\n";
			}
			fileInput.close();
			//Critical Region End Pt 1

			if (currentQueue.length() > 4 && currentQueue.at(jobIndex + 4) == '\n') { //If there are enough jobs in the queue and the current position of the queue is a new line, then remove it.
				fileContents.erase(fileContents.find(jobNum), 4);
			}
			else {
				fileContents.erase(fileContents.find(jobNum),5); //Remove the front of the queue if it is not a new line
			}

			//Critical Region Restart
			fileOutput.open(filename);
			fileOutput.write(fileContents.c_str(), fileContents.length()); //Re-write the queue back to the file
			fileOutput.close();
			//Critical Region Re-End
			cout << "Job: " << jobNum << " has been chosen via the scheduler." << endl;

			return stoi(jobNum); //Returns the job number
		}
	}
	
	//If a job wasn't found
	fileInput.close();
	//Critical Region End Pt2
	cout << "Selector: No available jobs to run";

	return -1;
}

void executeJob(int* jobID) //Takes in the job ID found by the selector above
{
	string jobComponents = to_string(*jobID); //Gets a string version of the job for output later
	int jobNum = stoi(jobComponents.substr(0, 2));
	int quantumTime = stoi(jobComponents.substr(2, 2)); //Pulls out the quantum time and the above pulls the number

	if (jobNum >= 1 && jobNum <= 30) { //If the job was between 1 and 30 inclusive, then it will simulate the SERVER_QUEUE
		//Execute job for one quantum cycle then sleep
		cout << "SERVER_QUEUE job: " << jobNum << " is being executed for 1 quantum (" << quantumTime - 1 << " quantums remain)\n";
		quantumTime--;
		sleep(1);

		stringstream jobNumInput, quantumTimeInput;
		jobNumInput << setw(2) << setfill('0') << jobNum;
		quantumTimeInput << setw(2) << setfill('0') << quantumTime;
		jobComponents = jobNumInput.str() + quantumTimeInput.str(); //After running for 1 quantum, the job will be re-assembled

		//Critical Region Start
		ifstream fileInput;
		fileInput.open(filename); //The job will be re-added to the queue by first loading the queue into a temp file

		string fileContents = "", lineContent = "";
		while (getline(fileInput, lineContent)) {
			fileContents += lineContent + "\n";
		}

		fileInput.close();
		//Crit Region End

		//Finds start and end positions in queue
		int start = 0, end = 0;
		for (int i = 0; i < SERVER_QUEUE; i++) { //The Server queue section is relocated
			start = fileContents.find_first_of('\n', start) + 1;
		}
		end = fileContents.find_first_of('\n', start);

		//Finds the location in the queue to re-insert the job
		string priorityLineLength = fileContents.substr(start, end - start);

		int entries = 0, values = 0;

		for (int i = 0; i < priorityLineLength.length(); i++) {//Counts out number of spaces that the job must be re-inserted over
			if (priorityLineLength[i] != ' ') {
				values++;
			}
		}
		entries = values / 4;

		if (entries < MAX_QUEUE) { //IF the number of entries is less than that of the job queue
			if (entries == 0) {
				fileContents.insert(end, jobComponents); //Reach the end of the queue and place the job there
			}
			else
			{
				jobComponents.insert(0, 1, ' ');
				fileContents.insert(end, jobComponents); // Otherwise restart the queue with the last job
			}


			//Critical Region Start
			ofstream fileOutput;
			fileOutput.open(filename);
			fileOutput.write(fileContents.c_str(), fileContents.length()); //Write the queue back out again
			fileOutput.close();
			//Crit Region End

			cout << "Moved Job " << jobNum << " in SERVER_QUEUE to back of queue\n";
		}
	}
	else if (jobNum >= 31 && jobNum <= 60) {
		cout << "POWER_USER_QUEUE: " << jobNum << " \"Press Ctrl +C\" to continue running\n"; //If the job is in the power user queue then simulate a power user queue
		signal(SIGINT, wake_up);
		sleep(15);
		cout << "Job " << jobNum << " is continuing because interrupt was pressed\n"; //Job waits until interrupt
		for (; quantumTime > 0; quantumTime--) {
			cout << "Job " << jobNum << " from POWER_USER_QUEUE is underway. (" + quantumTime << " quantums remaining)\n"; //Job executes
			sleep(1);
		}
		cout << "Job " << jobNum << " in POWER_USER_QUEUE has completed.\n";
	}
	else if (jobNum >= 61 && jobNum <= 100) {
		cout << "USER_QUEUE: " << jobNum << " is executing for 1 quantum (" << quantumTime - 1 << " quantums remaining) and sleep for 5 seconds.\n"; //Job in user queue executes
		quantumTime--;
		sleep(5);
		cout << "Job " << jobNum << "from USER_QUEUE has waked up and is now running until completion.";
		for (; quantumTime > 0; quantumTime--) {
			cout << "Job " << jobNum << " from USER_QUEUE is now executing with (" << quantumTime << " quantums remaining).\n"; // Job runs till completion
			sleep(1);
		}
		cout << "Job " << jobNum << " from USER_QUEUE has completed.\n";
	}
	else {
		cout << "Error: jobNum out of range exception.\n";
	}
}

//All below here was provided by the teacher for us to use and relates to Mutexes and locking down critical regions on the program, The content in everything above is my creation

void down(int *semid, char* semname)
{
	while (*semid = creat(semname, 0) == -1) /* && error == EACCES)*/
	{
		cout << "down " << semname << ": I am blocked.\n";
		sleep(1);
	}
}

void up(int semid, char* semname)
{
	close(semid);
	unlink(semname);
	cout << "up " << semname << ": I am waked up.\n";
}

void wake_up(int s)
{
	cout << "\nI will wake up now.\n";
	intr = 1;
}