using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DroneInputValidator  : MonoBehaviour
{
	public TMP_InputField InputFieldKronus, InputFieldEclipsia, InputFieldLyrion, InputFieldMistara, InputFieldHierarchy;
	public TMP_Text errorText;

	public void ValidateInputs(){
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

		if (kronus < lyrion || lyrion < mistara || mistara < eclipsia || eclipsia < hierarchy){
            errorText.text = "Кількість дронів має бути: Kronus ≥ Lyrion ≥ Mystara ≥ Eclipsia ≥ Fiora";
            return;
        }

		if ((kronus + eclipsia + lyrion + mistara + hierarchy) != 1000){
			errorText.text = "Сума має дорівнювати 1000!";
            return;
		}

		 errorText.text = "Успіх! Розподіл дронів прийнято";
	}
}
