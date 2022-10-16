using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyStringMethods : MonoBehaviour
{
	public static MyStringMethods singleton;

	/*	public static MyStringMethods Singleton {
			get {
				if (singleton == null) {
					singleton = new MyStringMethods();
				}
				return singleton;
			}
		}*/

	void Start() {
		singleton = this;
	}

	/// <summary> 서로 다른 문자열들을 하나로 접합 (최대 10개)
	/// </summary>
	public string Assemble_Strings(string str_1, string str_2, string str_3 = null, string str_4 = null, string str_5 = null, string str_6 = null, string str_7 = null, string str_8 = null, string str_9 = null, string str_10 = null) {
		int _count = 2;
		if (str_3 != null) {
			_count++;
			if (str_4 != null) {
				_count++;
				if (str_5 != null) {
					_count++;
					if (str_6 != null) {
						_count++;
						if (str_7 != null) {
							_count++;
							if (str_8 != null) {
								_count++;
								if (str_9 != null) {
									_count++;
									if (str_10 != null) {
										_count++;
									}
								}
							}
						}
					}
				}
			}
		}
		StringBuilder _sb = new StringBuilder(128 * _count); // 버퍼 수동 할당
		_sb.Append(str_1);
		_sb.Append(str_2);
		switch(_count) {
			case 3: _sb.Append(str_3); break;
			case 4: _sb.Append(str_3); _sb.Append(str_4); break;
			case 5: _sb.Append(str_3); _sb.Append(str_4); _sb.Append(str_5); break;
			case 6: _sb.Append(str_3); _sb.Append(str_4); _sb.Append(str_5); _sb.Append(str_6); break;
			case 7: _sb.Append(str_3); _sb.Append(str_4); _sb.Append(str_5); _sb.Append(str_6); _sb.Append(str_7); break;
			case 8: _sb.Append(str_3); _sb.Append(str_4); _sb.Append(str_5); _sb.Append(str_6); _sb.Append(str_7); _sb.Append(str_8); break;
			case 9: _sb.Append(str_3); _sb.Append(str_4); _sb.Append(str_5); _sb.Append(str_6); _sb.Append(str_7); _sb.Append(str_8); _sb.Append(str_9); break;
			case 10: _sb.Append(str_3); _sb.Append(str_4); _sb.Append(str_5); _sb.Append(str_6); _sb.Append(str_7); _sb.Append(str_8); _sb.Append(str_9); _sb.Append(str_10); break;
		}
		return _sb.ToString();
	}

}
