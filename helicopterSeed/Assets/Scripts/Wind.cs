using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    [SerializeField] SeedMovement seed;

    [SerializeField] Vector3 direction = new Vector3();
    [SerializeField] float strength = 0;

    [SerializeField] Vector2 StrengthMinMax = new Vector2(.5f, 5);
    [SerializeField] Vector2 changeTimeMinMax = new Vector2(.5f,5);
    [SerializeField] Vector2 blowingtimeMinMax = new Vector2(5,20);

    bool switchingDirections;
    bool blowing;
    bool wasBlowing = true;

    // Update is called once per frame
    void Update()
    {
        if (!blowing && !switchingDirections) {
            if (wasBlowing)
            {
                StartCoroutine(SwitchDirections());
            }
            else {
                StartCoroutine(Blowing());
            }
            wasBlowing = !wasBlowing;
        }
        seed.windDirection = direction;
        seed.windPower = strength;
    }

    IEnumerator Blowing()
    {
        blowing = true;
        yield return new WaitForSeconds(Random.Range(blowingtimeMinMax.x, blowingtimeMinMax.y));
        blowing = false;

    }

    IEnumerator SwitchDirections()
    {
        switchingDirections = true;

        float changeTimeStep = Random.Range(changeTimeMinMax.x, changeTimeMinMax.y)/200;
        float step = strength / 100;
        for (float i = strength; i>0; i -= step )
        {
            strength = i;
            yield return new WaitForSeconds(changeTimeStep);
        }
        strength = 0;
        
        //sets new direction and strength
        float strengthGoal = Random.Range(StrengthMinMax.x, StrengthMinMax.y);
        direction = Vector3.Normalize(new Vector3(Random.Range(0, 1000),0, Random.Range(0, 1000)));
        print("Wind Direction:" + direction);

        step = strengthGoal / 100;
        for (float i = strength; i < strengthGoal; i += step)
        {
            strength = i;
            yield return new WaitForSeconds(changeTimeStep);
        }
        switchingDirections = false;
    }
}
