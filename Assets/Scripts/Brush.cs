using System;
using System.Collections;
using UnityEngine;

public enum EDirectionMode
{
	Champion,
	LookAt,
}

public class Brush : MonoBehaviour
{
	[SerializeField] private Animator m_animator = default;
	[SerializeField] private BoxCollider m_collider = default;
	
	/// <summary>
	/// 풀이 흔들리는 정도
	/// </summary>
	[SerializeField] private float m_strength = 1.0f;
	
	/// <summary>
	/// 풀이 움직이는 시간 (1회당)
	/// </summary>
	[SerializeField] private float m_duration = 0.3f;
	
	/// <summary>
	/// 풀 방향 지정 모드
	/// </summary>
	[SerializeField] private EDirectionMode m_directionMode = EDirectionMode.LookAt;

	private Vector2 offset = default;
	private bool isAnimating = default;
	
	private static readonly int x = Animator.StringToHash("X");
	private static readonly int y = Animator.StringToHash("Y");
	
	private void OnTriggerEnter(Collider other)
	{
		BrushTouch brushTouch = other.GetComponent<BrushTouch>();
		
		if (brushTouch != null)
		{
			OnTouchEnterBrush(brushTouch);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		BrushTouch brushTouch = other.GetComponent<BrushTouch>();
		
		if (brushTouch != null)
		{
			OnTouchExitBrush(brushTouch);
		}
	}

	private void OnTouchEnterBrush(BrushTouch brushTouch)
	{
		if (isAnimating)
		{
			return;
		}
		
		Vector3 direction;
			
		switch (m_directionMode)
		{
			case EDirectionMode.Champion:
				Vector3 playerForward = brushTouch.transform.forward;
				direction = (playerForward - transform.forward).normalized;
					
				break;
			case EDirectionMode.LookAt:
				Vector3 vector = transform.position - brushTouch.transform.position;
				direction = vector.normalized;
					
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
			
		AddOffset(direction);
	}
	
	private void OnTouchExitBrush(BrushTouch brushTouch)
	{
		if (isAnimating)
		{
			return;
		}
		
		Vector3 direction;
			
		switch (m_directionMode)
		{
			case EDirectionMode.Champion:
				Vector3 playerForward = brushTouch.transform.forward;
				direction = -(playerForward - transform.forward).normalized;
					
				break;
			case EDirectionMode.LookAt:
				Vector3 vector = transform.position - brushTouch.transform.position;
				direction = -vector.normalized;
					
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
			
		AddOffset(direction);
	}

	private void AddOffset(Vector3 direction)
	{
		offset = new Vector2(direction.x, direction.z) * m_strength;
		
		if (!isAnimating)
		{
			StartCoroutine(AddOffsetCoroutine(m_duration));
		}
	}

	private IEnumerator AddOffsetCoroutine(float duration)
	{
		isAnimating = true;
		
		yield return FromToCoroutine(Vector2.zero, offset, duration);
		yield return FromToCoroutine(offset, offset * -0.9f, duration);
		yield return FromToCoroutine(offset * -0.9f, offset * 0.4f, duration);
		yield return FromToCoroutine(offset * 0.4f, offset * -0.2f, duration);
		yield return FromToCoroutine(offset * -0.2f, Vector2.zero, duration);
		
		isAnimating = false;
	}
	
	private IEnumerator FromToCoroutine(Vector2 from, Vector2 to, float duration)
	{
		float time = 0f;
		
		m_animator.SetFloat(x, from.x);
		m_animator.SetFloat(y, from.y);
		
		while (time <= duration)
		{
			time += Time.deltaTime;
			
			float progress = time / (duration);

			Vector2 lerped = Vector2.Lerp(from, to, progress);
			
			m_animator.SetFloat(x, lerped.x);
			m_animator.SetFloat(y, lerped.y);
			
			yield return null;
		}
		
		m_animator.SetFloat(x, to.x);
		m_animator.SetFloat(y, to.y);
	}
	
	private void Reset()
	{
		m_animator = GetComponent<Animator>();
		m_collider = GetComponent<BoxCollider>();

		if (m_collider == null)
		{
			Mesh mesh = GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;
			
			m_collider = gameObject.AddComponent<BoxCollider>();
			m_collider.size = mesh.bounds.size;
			m_collider.center = mesh.bounds.center;
		}
		
		gameObject.layer = 11;
		gameObject.tag = "Bush";
		
		gameObject.isStatic = true;
	}
}