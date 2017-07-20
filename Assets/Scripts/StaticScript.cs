using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticScript  {

	//Funcion copiada:
	//Pasados dos componentes, devuelve uno como copia
	private static T GetCopyOf<T>(this Component comp, T other) where T : Component
	{
		//Comprueba que los dos componentes sean del mismo tipo
		Type type = comp.GetType();
		if (type != other.GetType()) return null; // type mis-match
		//Los flags de que queremos copiar
		BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;

		PropertyInfo[] pinfos = type.GetProperties(flags);
		foreach (var pinfo in pinfos) {
			if (pinfo.CanWrite) {
				try {
					pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
				}
				catch { } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
			}
		}
		FieldInfo[] finfos = type.GetFields(flags);
		foreach (var finfo in finfos) {
			finfo.SetValue(comp, finfo.GetValue(other));
		}
		return comp as T;
	}

	public static T AddComponent<T>(this GameObject go, T toAdd) where T : Component {
		return go.AddComponent<T> ().GetCopyOf (toAdd) as T;
	}
}
