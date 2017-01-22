using UnityEngine;

[ExecuteInEditMode]
public class ReplacementShaderEffect : MonoBehaviour
{
	public string Tag = "";
	public Shader ReplacementShader = null;

	void OnEnable()
	{
		if (ReplacementShader == null)
			return;

		GetComponent<Camera>().SetReplacementShader(ReplacementShader, Tag);
	}

	void OnDisable()
	{
		GetComponent<Camera>().ResetReplacementShader();
	}
}
