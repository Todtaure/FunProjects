#include "stdafx.h"
#include <iostream>
#include <conio.h>

using namespace std;

void move(const int n, const int fromTower,
	const int toTower, const int spareTower)
{
	if (n>0)
	{
		move((n - 1), fromTower, spareTower, toTower);

		cout << "\t Move the Top Disk from Tower-" << fromTower
			<< " to Tower-" << toTower << "\n";

		move((n - 1), spareTower, toTower, fromTower);
	}
}

int main()
{
	cout << "\n\t **************   TOWERS OF HANOI   **************\n" << endl;
	cout << "\t The Mystery of Towers of Hanoi is as follows : \n" << endl;

	move(5, 1, 3, 2);

	cout << "\n\t *************************************************";

	getchar();
	return 0;
}