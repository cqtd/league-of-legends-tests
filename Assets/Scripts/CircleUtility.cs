using UnityEngine;

public class Circle
{
	GameObject instance;

	public void BeginDraw()
	{
		instance.gameObject.SetActive(true);
	}

	public void EndDraw()
	{
		instance.gameObject.SetActive(false);
	}
	
	const float planeOffset = 0.01f;

	public class Builder
	{
		string name;
		Transform target;
		Color color = Color.white;
		int intensity = 1;
		float size = 1.0f;

		public Builder(string name)
		{
			this.name = name;
		}

		public Builder SetTarget(Transform target)
		{
			this.target = target;
			return this;
		}

		public Builder SetColor(Color color)
		{
			this.color = color;
			return this;
		}

		public Builder SetIntensity(int intensity)
		{
			this.intensity = intensity;
			return this;
		}

		public Builder SetSize(float size)
		{
			this.size = size;
			return this;
		}

		public Circle Build(bool isActive = false)
		{
			var circle = new Circle();
			
			Material mat = Object.Instantiate(Resources.Load<Material>("MAT_DrawCircle"));
			mat.SetColor("_Color", color * intensity);
			
			GameObject spawned = Object.Instantiate(Resources.Load<GameObject>("DrawCircle"));
			spawned.transform.localScale = Vector3.one * size;
			spawned.transform.parent = target;
			spawned.transform.localPosition = Vector3.up * planeOffset;
			spawned.GetComponent<Renderer>().sharedMaterial = mat;
			spawned.SetActive(isActive);
			spawned.name = name;
			
			circle.instance = spawned;

			return circle;
		}
	}
}
