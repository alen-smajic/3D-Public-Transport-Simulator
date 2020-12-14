using System.Collections;
using UnityEngine;

// Diese Klasse ist an den Bussen und den ersten Schienenfahrzeugen angehängt und steuert deren Bewegung.
// Dabei wird eine Liste von Koordinaten abgearbeitet und das Fahrzeug jeweils zu dem nächsten Punkt aus 
// der Liste bewegt. Falls auf dem Weg eine Station angefahren wird, stoppt das Fahrzeug für 1 Sekunde
// an diesem Punkt. Erreicht das Fahrzeug das Ende der Liste, wird es zerstört.
public class VehicleMover : MonoBehaviour
{
    int targetIndex = 0; // Index des Punktes welcher als nächstes angefahren wird.
    int maxIndex;  // Letzter Index der Liste. Wird genutzt um das Fahrzeug am Ende zu zerstören.

    bool isWaiting = false;

    float VehicleMaxSpeed = 100f; // Geschwindigkeit des Fahrzeugs.

    void Start()
    {       
        transform.position = SortWay.PathsInRightOrder[0][0]; // Das Fahrzeug befindet sich am ersten Punkt der Liste.

        maxIndex = SortWay.MoveToTarget.Count - 1; // Letzter verfügbarer Index der Liste.
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
            return; // Fahrzeug hält an einer Station an.
        }

        // Das Fahrzeug bewegt sich jeweils zum nächsten Punkt der "MoveToTarget" Liste.
        transform.position = Vector3.MoveTowards(transform.position, SortWay.MoveToTarget[targetIndex], Time.deltaTime * VehicleMaxSpeed);

        if (transform.position == SortWay.MoveToTarget[targetIndex])
        {
            if (TranSportWayMarker.StationOrder.Contains(transform.position))
            {
                // Falls eine Station erreicht wurde, wird das Fahrzeug angehalten.
                StartCoroutine(Waiting());
            }
            if(targetIndex + 1 > maxIndex) 
            {
                // Falls das Fahrzeug den letzten Punkt der "MoveToTarget" Liste erreicht, wird es zerstört.
                Destroy(gameObject);
                return;
            }
            else if (SortWay.PathLastNode.Contains(transform.position))
            {
                // Falls das Fahrzeug den letzten Punkt eines Pfades erreicht hat, wird es zum nächsten Punkt
                // "telepotiert" statt diesen anzufahren. Dieser nächste Punkt ist zugleich der erste Punkt des neuen Pfades. Dies wird
                // gemacht um zu vermeiden dass das Fahrzeug zum nächsten Punkt ohne Straße/Schine fährt.
                int index = SortWay.MoveToTarget.IndexOf(transform.position);
                transform.position = SortWay.MoveToTarget[index + 1];
            }
            transform.LookAt(SortWay.MoveToTarget[targetIndex + 1]); // Rotiert das Fahrzeug in die Richtung des nächsten Punktes.

            targetIndex += 1;           
        }
    }

    // Diese Funktion hält ein Fahrzeug für 2 Sekunden an. Wird an Stationen aufgerufen.
    IEnumerator Waiting()
    {
        isWaiting = true;
        yield return new WaitForSeconds(2);
        isWaiting = false;
    }
}
