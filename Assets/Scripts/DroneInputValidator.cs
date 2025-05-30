using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DroneInputValidator : MonoBehaviour{
    public TMP_InputField InputFieldKronus, InputFieldEclipsia, InputFieldLyrion, InputFieldMistara, InputFieldHierarchy;
    public TMP_Text errorText;
    public DroneSubmitAndWait submitter;
    public DroneSpawner droneSpawner;

    void Start(){
        // Реєстрація події зміни значення кожного поля
        InputFieldKronus.onValueChanged.AddListener(delegate { OnAnyInputChanged(); });
        InputFieldEclipsia.onValueChanged.AddListener(delegate { OnAnyInputChanged(); });
        InputFieldLyrion.onValueChanged.AddListener(delegate { OnAnyInputChanged(); });
        InputFieldMistara.onValueChanged.AddListener(delegate { OnAnyInputChanged(); });
        InputFieldHierarchy.onValueChanged.AddListener(delegate { OnAnyInputChanged(); });
    }

    // Метод для відправки даних при натисканні Submit (залишається)
    public void ValidateInputs(){
        string[] values = new string[] {
            InputFieldKronus.text,
            InputFieldEclipsia.text,
            InputFieldLyrion.text,
            InputFieldMistara.text,
            InputFieldHierarchy.text
        };

        submitter.OnSubmitClicked(values);
    }

    // Метод, який автоматично викликається при зміні будь-якого поля
    void OnAnyInputChanged(){
        int kronus = 0, eclipsia = 0, lyrion = 0, mistara = 0, hierarchy = 0;

        bool valid = int.TryParse(InputFieldKronus.text, out kronus) &&
                     int.TryParse(InputFieldEclipsia.text, out eclipsia) &&
                     int.TryParse(InputFieldLyrion.text, out lyrion) &&
                     int.TryParse(InputFieldMistara.text, out mistara) &&
                     int.TryParse(InputFieldHierarchy.text, out hierarchy);

        if (!valid){
            errorText.text = "Введіть тільки числа!";
            return;
        }

        if (kronus < eclipsia || eclipsia < lyrion || lyrion < mistara || mistara < hierarchy){
            errorText.text = "Kronus ≥ Eclipsia ≥ Lyrion ≥ Mystara ≥ Hierarchy";
            return;
        }

        if ((kronus + eclipsia + lyrion + mistara + hierarchy) != 1000){
            errorText.text = "Сума має дорівнювати 1000!";
            return;
        }

        errorText.text = "Розподіл прийнято";

        // Передача кількості дронів для спавну
        droneSpawner.SpawnDrones(new int[] { kronus, eclipsia, lyrion, mistara, hierarchy });
    }
}
