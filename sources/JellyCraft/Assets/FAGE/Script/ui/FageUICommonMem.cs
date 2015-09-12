using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FageUICommonMem {
	public	const string DESTROIED	= "detroied";
	public	const string INTANTIATED= "instantiated";
	public	const string PAUSED		= "paused";

	private	Dictionary<string, int>		_dictionaryInt;
	private	Dictionary<string, bool>	_dictionaryBool;
	private	Dictionary<string, float>	_dictionaryFloat;
	private	Dictionary<string, double>	_dictionaryDouble;
	private	Dictionary<string, long>	_dictionaryLong;
	private	Dictionary<string, short>	_dictionaryShort;
	private	Dictionary<string, uint>	_dictionaryUint;
	private	Hashtable					_hashtable;
	private	string						_state;
	public	string						state { get { return _state; } }

	public	FageUICommonMem() {
		_dictionaryInt		= new Dictionary<string, int>();
		_dictionaryBool		= new Dictionary<string, bool>();
		_dictionaryFloat	= new Dictionary<string, float>();
		_dictionaryDouble	= new Dictionary<string, double>();
		_dictionaryLong		= new Dictionary<string, long>();
		_dictionaryShort	= new Dictionary<string, short>();
		_dictionaryUint		= new Dictionary<string, uint>();
		_hashtable			= new Hashtable();

		SetState (FageUICommonMem.DESTROIED);
	}

	public	void SetInt(string key, int value) {
		if (_dictionaryInt.ContainsKey(key))
			_dictionaryInt[key] = value;
		else
			_dictionaryInt.Add(key, value);
	}
	
	public	int GetInt(string key) {
		return _dictionaryInt[key];
	}
	
	public	bool GetInt(string key, ref int value) {
		if (_dictionaryInt.ContainsKey(key)) {
			value = _dictionaryInt[key];
			return true;
		}
		return false;
	}
	
	public	void SetBool(string key, bool value) {
		if (_dictionaryBool.ContainsKey(key))
			_dictionaryBool[key] = value;
		else
			_dictionaryBool.Add(key, value);
	}
	
	public	bool GetBool(string key) {
		return _dictionaryBool[key];
	}
	
	public	bool GetBool(string key, ref bool value) {
		if (_dictionaryBool.ContainsKey(key)) {
			value = _dictionaryBool[key];
			return true;
		}
		return false;
	}
	
	public	void SetFloat(string key, float value) {
		if (_dictionaryFloat.ContainsKey(key))
			_dictionaryFloat[key] = value;
		else
			_dictionaryFloat.Add(key, value);
	}
	
	public	float GetFloat(string key) {
		return _dictionaryFloat[key];
	}
	
	public	bool GetFloat(string key, ref float value) {
		if (_dictionaryFloat.ContainsKey(key)) {
			value = _dictionaryFloat[key];
			return true;
		}
		return false;
	}
	
	public	void SetDouble(string key, double value) {
		if (_dictionaryDouble.ContainsKey(key))
			_dictionaryDouble[key] = value;
		else
			_dictionaryDouble.Add(key, value);
	}
	
	public	double GetDouble(string key) {
		return _dictionaryDouble[key];
	}
	
	public	bool GetDouble(string key, ref double value) {
		if (_dictionaryDouble.ContainsKey(key)) {
			value = _dictionaryDouble[key];
			return true;
		}
		return false;
	}
	
	public	void SetLong(string key, long value) {
		if (_dictionaryLong.ContainsKey(key))
			_dictionaryLong[key] = value;
		else
			_dictionaryLong.Add(key, value);
	}
	
	public	long GetLong(string key) {
		return _dictionaryLong[key];
	}
	
	public	bool GetLong(string key, ref long value) {
		if (_dictionaryLong.ContainsKey(key)) {
			value = _dictionaryLong[key];
			return true;
		}
		return false;
	}
	
	public	void SetShort(string key, short value) {
		if (_dictionaryShort.ContainsKey(key))
			_dictionaryShort[key] = value;
		else
			_dictionaryShort.Add(key, value);
	}
	
	public	short GetShort(string key) {
		return _dictionaryShort[key];
	}
	
	public	bool GetShort(string key, ref short value) {
		if (_dictionaryShort.ContainsKey(key)) {
			value = _dictionaryShort[key];
			return true;
		}
		return false;
	}
	
	public	void SetUint(string key, uint value) {
		if (_dictionaryUint.ContainsKey(key))
			_dictionaryUint[key] = value;
		else
			_dictionaryUint.Add(key, value);
	}
	
	public	uint GetUint(string key) {
		return _dictionaryUint[key];
	}
	
	public	bool GetUint(string key, ref uint value) {
		if (_dictionaryUint.ContainsKey(key)) {
			value = _dictionaryUint[key];
			return true;
		}
		return false;
	}
	
	public	void SetObject(string key, object value) {
		if (_hashtable.ContainsKey(key))
			_hashtable[key] = value;
		else
			_hashtable.Add(key, value);
	}
	
	public	object GetObject(string key) {
		return _hashtable[key];
	}
	
	public	bool GetObject(string key, ref object value) {
		if (_hashtable.ContainsKey(key)) {
			value = _hashtable[key];
			return true;
		}
		return false;
	}

	public	void SetState(string state) {
		_state = state;
	}
}
