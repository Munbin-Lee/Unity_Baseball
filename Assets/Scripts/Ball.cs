using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private bool canSwing = false;
    private bool throwing = true;
    public GameObject yourBall;
    public Animator animator;
    public SpriteRenderer hitMessage;
    public Sprite Strike;
    public Sprite Foul;
    public Sprite Hit;
    public Sprite Homerun;
    public GameObject MoneyObject;
    private TMP_Text MoneyText;
    public GameObject UpgradeObject;
    private TMP_Text UpgradeText;
    private float Scale = 1f;
    [SerializeField] private float Money = 0f;
    private float spd = 1f;
    private float AllowdGap = 1f;
    private float MoneyUpgrade = 10f;
    private float SpeedUpgrade = 10f;
    private float GapUpgrade = 10f;
    private static readonly int ThrowTrigger = Animator.StringToHash("ThrowTrigger");

    private void Start()
    {
        MoneyText = MoneyObject.GetComponent<TMP_Text>();
        UpgradeText = UpgradeObject.GetComponent<TMP_Text>();
        
        RefreshMoneyUI();
        RefreshUpgradeUI();
    }

    private void RefreshMoneyUI()
    {
        MoneyText.text = "돈 : " + ((int)Money).ToString() + "원";
    }

    private void RefreshUpgradeUI()
    {
        var txt = "수익 강화 : " + ((int)MoneyUpgrade).ToString() + "원\n\n";
        txt += "속도 강화 : " + ((int)SpeedUpgrade).ToString() + "원\n\n";
        txt += "판정 강화 : " + ((int)GapUpgrade).ToString() + "원";
        UpgradeText.text = txt;
    }

    public void UpgradeMoney()
    {
        Debug.Log("Fuck");
        if (Money < MoneyUpgrade) return;
        Money -= MoneyUpgrade;
        Scale *= 1.1f;
        MoneyUpgrade *= 1.1f;
        RefreshMoneyUI();
        RefreshUpgradeUI();
    }

    public void UpgradeSpeed()
    {
        if (Money < SpeedUpgrade) return;
        Money -= SpeedUpgrade;
        spd += 0.1f;
        SpeedUpgrade *= 1.1f;
        RefreshMoneyUI();
        RefreshUpgradeUI();
    }

    public void UpgradeGap()
    {
        if (Money < GapUpgrade) return;
        Money -= GapUpgrade;
        AllowdGap *= 1.1f;
        GapUpgrade *= 1.1f;
        RefreshMoneyUI();
        RefreshUpgradeUI();
    }

    private void ThrowBall()
    {
        animator.SetTrigger(ThrowTrigger);
        transform.position = new Vector3(7.35f, 2.75f, 0);
        throwing = true;
    }

    private void ThrowBallReal()
    {
        canSwing = true;
        throwing = false;
    }

    private void Swing()
    {
        canSwing = false;
        var position = transform.position;
        var diff = math.abs(position.y + 3.45);
        if (diff < 0.05f * AllowdGap)
        {
            hitMessage.sprite = Homerun;
            Money += 2 * Scale;
        }
        else if (diff < 0.1f * AllowdGap)
        {
            hitMessage.sprite = Hit;
            Money += 1 * Scale;
        }
        else if (diff < 0.2f * AllowdGap)
        {
            Money += 0.5f * Scale;
            hitMessage.sprite = Foul;
        }
        else
        {
            hitMessage.sprite = Strike;
        }

        RefreshMoneyUI();

        yourBall.transform.position = position;
        ThrowBall();
    }

    private void Update()
    {
        if (canSwing)
        {
            var transform1 = transform;
            transform1.position = new Vector3(7.35f, transform1.position.y - 0.01f * spd, 0);
        }

        if (Input.GetKeyDown("space"))
        {
            if (canSwing) Swing();
        }

        if (throwing && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            ThrowBallReal();
        }
    }
}