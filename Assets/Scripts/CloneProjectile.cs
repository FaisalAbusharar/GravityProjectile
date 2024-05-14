using Cinemachine;
using UnityEngine;

public class CloneProjectile : MonoBehaviour
{



    [SerializeField] Transform spawnPos;// the position where to clone sphere
    [SerializeField] GameObject sphere;// gameobject want to spawn
    [SerializeField] GameObject storeTrail; // where the old trails will be stored
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] GameObject storeSpheres; // where old spheres will be stored


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D)) {

            while (storeSpheres.transform.childCount != 0) // While the amount of children inside the object is not 0
            {
                Destroy(storeSpheres.transform.GetChild(0).gameObject); // Destroy the child (sphere)
            }

        }
    }

    public void CloneBall()
        {
            GameObject newSphere = Instantiate(sphere, spawnPos.position, spawnPos.rotation); // create a new sphere at origin 
            GameObject ghostSphere = Instantiate(sphere, sphere.transform.position, spawnPos.rotation); // create a ghost sphere at the same location of the old ball
            if (ghostSphere.transform.position == spawnPos.position){ Destroy(ghostSphere);}

            ghostSphere.GetComponent<CircleCollider2D>().enabled = false; // disable the ball's collider so its a ghost
            ghostSphere.GetComponent<ProjectileScript>().enabled = false;    

            ghostSphere.transform.SetParent(storeSpheres.transform); // Set the parent to the gameobject, so its cleaner.
            TrailRenderer trail = sphere.GetComponentInChildren<TrailRenderer>(); // Grab the Trail from the gameobject's children

            trail.transform.SetParent(storeTrail.transform); // Change the parent to the gameobject, so its cleaner.
            
            // Create random RGB Values for the ball & trail colors
            float R = Random.value;
            float G = Random.value;
            float B = Random.value;
            
            // Create it here.
            Color color = new Color(R,G,B);

            // Gradient Code, not really important
            var gradient = new Gradient();

            var colors = new GradientColorKey[2];
            colors[0] = new GradientColorKey(color, 1.0f);
            colors[1] = new GradientColorKey(color, 1.0f);
            
            // Alphas (The Opacity)
            var alphas = new GradientAlphaKey[2];
            alphas[0] = new GradientAlphaKey(1.0f, 0.0f);
            alphas[1] = new GradientAlphaKey(0.0f, 1.0f);

            gradient.SetKeys(colors, alphas);

            trail.colorGradient = gradient;
            ghostSphere.GetComponent<SpriteRenderer>().color = new Color(R,G,B, 0.3f); // Make the opacity of the ball low to appear ghostly.
            Destroy(sphere); // Remove old sphere.

            // Change what the VC is looking at.
            virtualCamera.Follow = newSphere.transform;
            virtualCamera.LookAt = newSphere.transform;

            sphere = newSphere;
            newSphere.transform.name = "Projectile";
        
        }
    
}
