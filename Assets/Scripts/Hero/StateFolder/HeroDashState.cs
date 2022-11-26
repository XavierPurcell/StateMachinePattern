using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroDashState : HeroOnGroundState
{


    //This isnt used


    //Coroutine CRDashHolder;

    //float DashTimer = 0.6f;
    [SerializeField]
    float ObjectActivationTimer = 0.01f;
    [SerializeField]
    float DashSpeed = 0;

    /* float OldMaxSpeed = 0;
     float OldInitialSpeed = 0;*/

    public void DashingMethod()
    {


        PlayerData.RB2D.gravityScale = 0;
        PlayerData.RB2D.velocity = new Vector2(DashSpeed, 0);
        PlayerData.TR.enabled = true;
        //CRDashHolder = StartCoroutine(CRDashMethod(DashTimer));

        /* OldInitialSpeed = PlayerData.Parent.MaxSpeed;
         OldMaxSpeed = PlayerData.Parent.InitialSpeed;
         PlayerData.Parent.MaxSpeed = 50;
         PlayerData.MaxSpeed = 50;
         PlayerData.Parent.InitialSpeed = 50;
         PlayerData.InitialSpeed = 50;*/

        //PlayerData.DashSFX.textureSheetAnimation.SetSprite(0, PlayerData.DashSprite);
        //Instantiate(dashParticle, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), Quaternion.identity, dashContainer.transform);
    }

    IEnumerator CRDashMethod(float DashTimer)
    {

        for (int i = 0; i < PlayerData.DashSFX.Count; i++)
        {

            SpriteRenderer sp = PlayerData.DashSFX[i].GetComponent<SpriteRenderer>();
            PlayerData.DashSFX[i].transform.position = new Vector3(PlayerData.transform.position.x, PlayerData.transform.position.y, PlayerData.transform.position.z);
            sp.sprite = PlayerData.CurrentSprite;
            sp.flipX = PlayerData.SpriteFlippedX;

            PlayerData.DashSFX[i].SetActive(true);
            yield return new WaitForSeconds(ObjectActivationTimer);
        }




        /*  PlayerData.Parent.MaxSpeed = OldMaxSpeed;
          PlayerData.MaxSpeed = OldMaxSpeed;
          PlayerData.Parent.InitialSpeed = OldInitialSpeed;
          PlayerData.InitialSpeed = OldInitialSpeed;
  */

        PlayerData.RB2D.gravityScale = PlayerData.DownGravity;
        PlayerData.CurrentlyDashing = false;
        PlayerData.RB2D.velocity = new Vector2(0, 0);

        // CRDashHolder = null;

        yield return new WaitForSeconds(1f);
        for (int i = 0; i < PlayerData.DashSFX.Count; i++)
        {
            if (PlayerData.DashSFX[i].activeInHierarchy)
            {
                PlayerData.DashSFX[i].SetActive(false);
            }
        }
        PlayerData.TR.enabled = false;

    }

}
