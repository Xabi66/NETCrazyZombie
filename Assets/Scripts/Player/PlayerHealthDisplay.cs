using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthDisplay : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealth health; //Enlace a la vida
    [SerializeField] private TMP_Text txtHealth; //Txt de la vida
    [SerializeField] private Image healthBarImage;

    //Actualiza el valor de la vida al spawnear
    public override void OnNetworkSpawn()
    {
        if (!IsClient) return;

        health.CurrentHealth.OnValueChanged += HandleHealthChanged; //Se suscribe al evento cuando cambia de vida
        HandleHealthChanged(0, health.CurrentHealth.Value); //Actualiza el valor al iniciar
    }
    
    //Elimina la llamada a HandleHealt al despawnear
    public override void OnNetworkDespawn()
    {
        if (!IsClient) return;

        health.CurrentHealth.OnValueChanged -= HandleHealthChanged; //Se desuscribe del evento al despawnear
    }
    //Cambia la vida actual cuando corresponda
    private void HandleHealthChanged(int oldHealth, int newHealth)
    {
        txtHealth.text = newHealth.ToString();

        healthBarImage.rectTransform.localScale = new Vector3((float)newHealth / 100.0f, 1);
        
        float healthPercent = (float)newHealth / health.MaxHealth;

        Color healthBarColor = new Color(1 - healthPercent, healthPercent, 0);
        healthBarImage.color = healthBarColor;
    }
}