using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    public bool spawnerOn = false;
    public bool finalAreaComplete = false;
    [SerializeField] float height = 10;
    [SerializeField] float width = 20; 
    // [SerializeField] 
    [SerializeField] List<GameObject> enemyPrefabs;

    [SerializeField] int numberToSpawn = 5;
    [SerializeField] int increaseNumberToSpawnPerRound = 2;
    [SerializeField] int totalRounds = 10;

    int round = 0;
    // private Canv
 
    // Update is called once per frame
    void Update()
    {
        // spawner has no enemies left and spawner is on
        if (transform.childCount == 0 && spawnerOn && round < totalRounds)
        {
            // spawn enemies 
            round++;
            if (round == totalRounds) {
                GameObject hamburg = Instantiate(enemyPrefabs[1], new Vector3(this.transform.position.x, this.transform.position.y)
                        , Quaternion.identity, transform);
                    hamburg.transform.localScale = new Vector3(3f, 3f, 1f);
            } else {
                for (int i = 0; i < numberToSpawn; i++) {
                    int enemyType = Random.Range(0, 4);
                    Instantiate(enemyPrefabs[enemyType], new Vector3(this.transform.position.x + GetModifier(width - 1f), this.transform.position.y + GetModifier(height - 1f))
                        , Quaternion.identity, transform);
                }
            }
            numberToSpawn += increaseNumberToSpawnPerRound;
        }
        finalAreaComplete = (round == totalRounds) && (transform.childCount == 0);
        if (finalAreaComplete) {
            // if ()
            SceneManager.LoadScene("Victory Scene Blue");

        }
    }
 
    float GetModifier(float d)
    {
        float modifier = Random.Range(0f, 1f) * d/2f;
        if (Random.Range(0, 2) > 0) {
            return -modifier;
        } else {
            return modifier;
        }
    }
}
