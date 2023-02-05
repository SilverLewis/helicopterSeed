using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    [SerializeField] SeedMovement seed;
    [SerializeField] WindZone windZone;

    [SerializeField] Vector3 direction = new Vector3();
    [SerializeField] float strength = 0;

    [SerializeField] Vector2 StrengthMinMax = new Vector2(.5f, 5);
    [SerializeField] Vector2 changeTimeMinMax = new Vector2(.5f,5);
    [SerializeField] Vector2 blowingtimeMinMax = new Vector2(5,20);

    [SerializeField] AudioSource windSound;
    [SerializeField] Vector2 windVolumeMinMax = new Vector2(.05f, .3f);
    [SerializeField] Vector2 windPitchMinMax = new Vector2(.7f, 1.3f);
    [SerializeField] float windPanDelta = -.7f;

    float soundDelta;
    float pitchDelta;

    bool switchingDirections;
    bool blowing;
    bool wasBlowing = true;

    private void Start()
    {
        soundDelta = windVolumeMinMax.y - windVolumeMinMax.x;
        pitchDelta = windPitchMinMax.y - windPitchMinMax.x;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = seed.gameObject.transform.position; 

        windZone.windMain = strength*2;
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

        float soundPrecent = strength / StrengthMinMax.y;
        windSound.volume = windVolumeMinMax.x + soundDelta * soundPrecent;
        windSound.pitch = windPitchMinMax.x + pitchDelta * soundPrecent;
        windSound.panStereo = 0 + windPanDelta * getOffsetPercent();
        print(windSound.pitch);

    }

    float getOffsetPercent() {
        Vector2 forward = new Vector2(seed.forwardVector.x,seed.forwardVector.z).normalized;
        Vector2 wind = new Vector2(direction.x,direction.z).normalized;
        float sign = Mathf.Sign(Vector2.Dot(forward, wind));
        float percent =1- (Vector2.Angle(forward, wind) / 180);

        print("here: "+ sign + ":"+percent);
        //forward vs win

        return sign*percent;
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

        direction = Vector3.Normalize(new Vector3(Random.Range(-1000, 1000), Mathf.Max(Random.Range(-4000, 1000),0), Random.Range(-1000, 1000)));
            
        transform.rotation =  Quaternion.LookRotation(direction,Vector3.up);

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
