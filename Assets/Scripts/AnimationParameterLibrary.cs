using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AnimationParameterLibrary
{
	readonly Dictionary<string, int> hashes;
    
	public AnimationParameterLibrary(IEnumerable<string> names)
	{
		hashes = new Dictionary<string, int>();

		foreach (string name in names)
		{
			hashes[name] = Animator.StringToHash(name);
		}
	}

	public int GetHash(string name)
	{
		return !hashes.TryGetValue(name, out int hash) ? 0 : hash;
	}
}