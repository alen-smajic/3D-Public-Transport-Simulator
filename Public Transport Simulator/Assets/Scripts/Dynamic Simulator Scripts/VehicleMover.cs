using System.Collections;
using UnityEngine;

/*
    Copyright (c) 2020 Alen Smajic

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
*/

/// <summary>
/// This script controlls the movement of the busses and the first train wagon. To move the vehicles, a list
/// of coordinates is provided which contains unity coordinates. The vehicle is basically moving from point to 
/// point. If the vehicle reaches a point where a station is located, it stops for 1 second. When the vehicle
/// reaches the end of the list, it destroys itself.
/// </summary>
public class VehicleMover : MonoBehaviour
{
    int targetIndex = 0; // Index of the next point which we are moving to.
    int maxIndex;  

    bool isWaiting = false;

    float VehicleMaxSpeed = 100f; 

    void Start()
    {       
        transform.position = SortWay.PathsInRightOrder[0][0]; // Starting point for the vehicle.

        maxIndex = SortWay.MoveToTarget.Count - 1; 
    }

    void Update()
    {
        if (IngameMenu.DarkModeOn)
        {
            transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(1).gameObject.SetActive(false);
        }
        if (isWaiting)
        {
            return; // Vehicle doesnt move uppon reaching a station.
        }

        // The vehicles moves to the next point from the "MoveToTarget" list.
        transform.position = Vector3.MoveTowards(transform.position, SortWay.MoveToTarget[targetIndex], Time.deltaTime * VehicleMaxSpeed);

        if (transform.position == SortWay.MoveToTarget[targetIndex])
        {
            if (TranSportWayMarker.StationOrder.Contains(transform.position))
            {
                // If the vehicle reaches a station, it stops.
                StartCoroutine(Waiting());
            }
            if(targetIndex + 1 > maxIndex) 
            {
                // If the vehicle reaches the last point of the list, it destroys itself.
                Destroy(gameObject);
                return;
            }
            else if (SortWay.PathLastNode.Contains(transform.position))
            {
                // If the road/railroad consists of more than one part we have to teleport the vehicle to the other part upon reaching.
                // the end of one part.
                int index = SortWay.MoveToTarget.IndexOf(transform.position);
                transform.position = SortWay.MoveToTarget[index + 1];
            }
            transform.LookAt(SortWay.MoveToTarget[targetIndex + 1]); // Rotates te vehicle in the right direction.

            targetIndex += 1;           
        }
    }

    /// <summary>
    /// Stops the vehicle movement for 2 seconds.
    /// </summary>
    /// <returns></returns>
    IEnumerator Waiting()
    {
        isWaiting = true;
        yield return new WaitForSeconds(2);
        isWaiting = false;
    }
}
