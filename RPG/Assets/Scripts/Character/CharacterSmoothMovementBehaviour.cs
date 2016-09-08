/*
 * Esse script movimenta, suavemente, um GameObject em direções verticais e horizontais.
 * As coordenadas dos destinos DEVEM ser valores inteiros.
 * Para iniciar uma movimentação invoque o método SetDestination() com a direção e a velocidade desejada.
 * Se o GameObject estiver habilitado a mover-se, ou seja, estiver parado, ele iniciará a movimentação
*/

#define __DEBUG__

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterSmoothMovementBehaviour : ICharacterMovementBehaviour {

	#region TESTED
	#region FIELDS

	private GameObject _character;                            // GameObject que receberá o comportamento
	private Vector3 _destination;		  					  // Direção para a qual o player deve andar
	private float _speed;	     	 					      // Velocidade em tiles/seg
	private bool _moving;		                              // Flag de movimentação usada e modificado em Move()
	private Rect _mapLimits;							      // Coordenada das bordas do mapa

	#endregion

	public CharacterSmoothMovementBehaviour (GameObject toMove, float speed, Rect mapRect) {
		this._character = toMove;
		this._destination = _character.transform.position;
		this._speed = speed;
		this._moving = false;
		this._mapLimits = mapRect;
	}

	void Awake () {
		
	}

	void Start () {

	}

	void Update () {
		Debug.Log ("asdasf");
		Move ();
	}

	void FixedUpdate () {
	}

	public override Vector3 GetDestination() {
		return _destination;
	}

	public override float GetSpeed () {
		return _speed;
	}

	public override bool SetSpeed (float speed) {
		if (_moving)
			return false;
		this._speed = speed;
		return true;
	}

	public override Vector3 GetPosition() {
		return _character.transform.position;
	}

	public override bool IsMoving() {
		return _moving;
	}

	#endregion TESTED

	public override bool SetDestination (Vector3 destination, float speed) {
		if (_moving)
			return false;

		if (!CanMoveTo (destination))
			return false;

		SetSpeed (speed);
		_destination = destination;
		_moving = true;
		return true;
	}

	// Como SetDestination() sempre verifica a validade do destino, o método move mover sem fazer quaisquer alterações
	private void Move() {
		Vector3 toAdd = _destination - _character.transform.position;

		Vector3 step = toAdd.normalized * _speed * Time.deltaTime;

		if (toAdd.magnitude <= step.magnitude) {
			_character.transform.position = _destination;
			_moving = false;
		}
		else {
			_moving = (_character.transform.position += step) == _destination ? false : true;
		}
	}

	#region TESTED
	// Verifica se pode mover para um POSIÇÃO no mapa, e não para um DIREÇÃO
	bool CanMoveTo (Vector3 destination) {

		// Verificando se a posição de destino já não é a posição atual;
		if (destination == _character.transform.position) {
			#if __DEBUG__
			Debug.LogWarning("Trying to move for the current Position");
			#endif
			return false;
		}

		// Verificando se as coordenadas do destino são inteiros
		var destinationInt = new Vector3 ((int) destination.x, (int) destination.y, (int) destination.z);
		if (destination != destinationInt) {
			// As coordenadas não são inteiras
			#if __DEBUG__
			Debug.LogError ("Invalid destination \"" + destination + "\". Only can move to integer coordinates");
			UnityEditor.EditorApplication.isPlaying = false;
			#endif
			return false;
		}


		// Verificando se a posição está fora do mapa
		// Lembrando que o pivô do player está no canto inf esq

		const float tileSize = GameGlobalConfigurations.TILE_SIZE;

		if (destination.x < _mapLimits.xMin ||
			(destination.x + tileSize) > _mapLimits.xMax ||
			destination.y < _mapLimits.yMin ||
			(destination.y + tileSize) > _mapLimits.yMax) {

			#if __DEBUG__
			Debug.LogError ("The direction " + destination + " moves the character away of the map");
			UnityEditor.EditorApplication.isPlaying = false;
			#endif
			return false;
		}


		// Verificando se o destino está na mesma linha ou na mesma coluna que a posição atual
		var diff = (destination - _character.transform.position).normalized;


		if (diff == Vector3.up) {
		} else if (diff == Vector3.down) {
		} else if (diff == Vector3.left) {
		} else if (diff == Vector3.right) {
		} else {
			#if __DEBUG__
			Debug.LogError ("Invalid destination \"" + destination + "\". Can only move horizontally or vertically");
			Debug.LogError ("Current position \"" + _character.transform.position + "\".");
			UnityEditor.EditorApplication.isPlaying = false;
			#endif
			return false;
		}

		// O destino é diferente da posição atual
		// As coordenadas do vetor são inteiros
		// O destino está dentro do mapa
		// O destino está enfileirado com a posição atual

		// TODO Checar as colisões

		return true;
	}
	#endregion
}
