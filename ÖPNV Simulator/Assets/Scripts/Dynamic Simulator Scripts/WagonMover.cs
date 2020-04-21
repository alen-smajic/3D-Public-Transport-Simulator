using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// Dieses Skript ist ähnlich zu "VehicleMover" jedoch ist dieses auf die Wagon Fahrzeuge ausgerichtet und bewegt
// die jeweiligen Wagon Fahrzeuge abhängig von dem davor generierten Fahrzeug. Das Fahrzeug welches dieses Skript
// enthält, folgt dem zuletzt generiertem Obekt.
public class WagonMover : MonoBehaviour
{
    int targetIndex = 0; // Index des Punktes welcher als nächstes angefahren wird.

    bool isWaiting = false;

    float VehicleMaxSpeed = 100f; // Geschwindigkeit des Fahrzeugs.

    GameObject WagonToFollow; // Fahrzeug welchem gefolgt wird.

    Vector3 lastWagonPos = Vector3.zero;
    Vector3 curWagonPos = new Vector3(0, 0, 1);

    float velocity = 1f; // Wird genutzt für die Geschwindigkeitsanpassung.

    void Start()
    {
        transform.position = SortWay.PathsInRightOrder[0][0]; // Das Fahrzeug befindet sich am ersten Punkt der Liste.

        List<GameObject> allGameObjects = new List<GameObject>();
        Scene scene = SceneManager.GetActiveScene();
        scene.GetRootGameObjects(allGameObjects);

        // Als Objekt welchem gefolgt werden soll wird das vorletzte Objekt der Scene genommen. 
        WagonToFollow = allGameObjects[allGameObjects.Count - 2];
    }

    void Update()
    {   
        if(WagonToFollow == null)
        {
            gameObject.Destroy(); // Falls das Objekt welches verfolgt wird nicht mehr existiert, wird das aktuelle Fahrzeug ebenfalls zerstört.
            return;
        }

        // Hier wird die Entfärnung des aktuellen Fahrzeugs zu dem verfolgtem Fahrzeug gemessen.
        float distance = Vector3.Distance(WagonToFollow.transform.GetChild(0).GetChild(1).position, transform.GetChild(0).GetChild(0).position);
        if(Vector3.Distance(WagonToFollow.transform.position, transform.position) < 30)
        {
            velocity = .1f;
        }
        else if(distance > 20)
        {
            // Falls die Entfärnung zwischen den Fahrzeugen zu groß ist, wird beschleunigt um dieses einzuholen.
            velocity = 1.2f;
        }
        else if(distance < 10)
        {
            // Falls die Entfärnung zwischen den Fahrzeugen zu klein ist, wird verlangsamt um die Entfärnung anzupassen.
            velocity = .8f;
        }
        else
        {
            // Falls die Entfärnung stimmt, wird die aktuelle Geschwindigkeit gehalten.
            velocity = 1f;
        }

        // Dieser Abschnitt misst ob das verfolgte Fahrzeug stehen geblieben ist. Falls dies der Fall ist, wird das
        // Wagonfahrzeug ebenfalls stehen bleiben und sich erst dann bewegen wenn sich das verfolgte Fahrzeug
        // erneut bewegt.
        curWagonPos = WagonToFollow.transform.position;
        if( curWagonPos == lastWagonPos)
        {
            // Verfolgtes Fahrzeug ist stehengeblieben.
            isWaiting = true;
        }
        else
        {
            // Verfolgtes Fahrzeug bewegt sich.
            isWaiting = false;
        }
        lastWagonPos = curWagonPos;
        if (isWaiting)
        {
            return; // Das Wagon Fahrzeug bewegt sich nicht mehr.
        }

        // Das Wagon Fahrzeug bewegt sich jeweils zum nächsten Punkt der "MoveToTarget" Liste.
        transform.position = Vector3.MoveTowards(transform.position, SortWay.MoveToTarget[targetIndex], Time.deltaTime * VehicleMaxSpeed * velocity);
        if (transform.position == SortWay.MoveToTarget[targetIndex])
        {
            if (SortWay.PathLastNode.Contains(transform.position))
            {
                // Falls das Wagon Fahrzeug den letzten Punkt eines Pfades erreicht hat, wird es zum nächsten Punket
                // "telepotiert" statt diesen anzufahren. Dieser nächste Punkt ist zugleich der erste Punkt des neuen Pfades. Dies wird
                // gemacht um zu vermeiden dass das Fahrzeug zum nächsten Punkt ohne Straße/Schine fährt.
                int index = SortWay.MoveToTarget.IndexOf(transform.position);
                transform.position = SortWay.MoveToTarget[index + 1];
            }
            transform.LookAt(SortWay.MoveToTarget[targetIndex + 1]); // Rotiert das Fahrzeug in die Richtung des nächsten Punktes.

            targetIndex += 1;
        }
    }
}
