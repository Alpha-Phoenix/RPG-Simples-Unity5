#define __DEBUG__

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class PlayerMovementBehaviour : MonoBehaviour {

	#region TESTED
	#region PROPERTIES
	public enum PlayerDirection {
		UP    = 1,
		RIGHT = 2,
		DOWN  = 3,
		LEFT  = 4
	}

	public enum PlayerState {
		STOPPED = 1,
		WALKING = 2,
		RUNNING = 3
	}

	private PlayerDirection _direction = PlayerDirection.UP; // Direção para a qual o player iniciará virado
	private PlayerState _state = PlayerState.STOPPED;		 // Estado atual (PARADO)
	private Vector3 _destination;		  					 // Direção para a qual o player deve andar
	private const float initialSpeed = 3;					 // Velocidade default da animação de andar/correr
	private const float speedMultiplier = 1.5f;				 // Multiplicador quando estiver correndo
	private const float speed = initialSpeed;        	     // Velocidade de animação
	private const float _tileSize = 1;            			 // Largura e altura de um tile em Unity units
	private const float _delayTime = 0.15f;			  		 // O player só se movimenta para uma determinada direção se o keyRecord dessa
									  						 // direção for igual ao delayTime
	private bool _moving = false;                            // Flag de movimentação usado e modificado em Move()
	private bool _running = false;							 // Flag de movimentação acelerada
	private Animator animator;		  						 // Controlador de animações
	private Rect mapLimits;									 // Coordenada das bordas do mapa

	#endregion

	void Start () {
		_destination = transform.position;
		animator = GetComponent<Animator> ();

		// Necessário para encontrar as dimensões do mapa atual
		// Deve haver um ÚNICO mapa no jogo e este DEVE conter a TAG Map
		var mapSpriteRenderer = GameObject.FindGameObjectWithTag("Map").GetComponent<SpriteRenderer>() as SpriteRenderer;
		_moving = false;

		mapLimits.xMin = mapSpriteRenderer.bounds.min.x;
		mapLimits.yMin = mapSpriteRenderer.bounds.min.y;
		mapLimits.xMax = mapSpriteRenderer.bounds.max.x;
		mapLimits.yMax = mapSpriteRenderer.bounds.max.y;
	}

	// Update is called once per frame
	void FixedUpdate () {
		Move ();
		UpdatePlayerAnimationState ();
	}

	public Vector3 GetPosition() {
		return transform.position;
	}

	public bool SetDestination (Vector3 newDestination, bool run) {
		if (_moving)
			return false;

		if (!CanMoveTo (newDestination))
			return false;

		_destination = newDestination;
		_moving = true;
		_running = run;

		return true;
	}

	public bool IsMoving() {
		return _moving;
	}

	private void UpdatePlayerAnimationState () {
		
		if (!IsMoving ())
			_state = PlayerState.STOPPED;
		else
			_state = _running ? PlayerState.RUNNING : PlayerState.WALKING;
		
		animator.SetInteger ("PlayerState", (int) _state);
		animator.SetInteger ("PlayerDirection", (int) _direction);
	}

	#endregion TESTED

	// Como SetDestination() sempre verifica a validade do destino, o método move mover sem fazer quaisquer alterações
	private void Move() {
		Vector3 toAdd = _destination - transform.position;

		Vector3 step = _running ?
			toAdd.normalized * speed * speedMultiplier * Time.deltaTime :
			toAdd.normalized * speed * Time.deltaTime;

		if (toAdd.magnitude <= step.magnitude) {
			transform.position = _destination;
			_moving = false;
		}
		else {
			_moving = (transform.position += step) == _destination ? false : true;
		}

		toAdd = toAdd.normalized;

		if (toAdd == Vector3.left) _direction = PlayerDirection.LEFT;
		else if (toAdd == Vector3.right) _direction = PlayerDirection.RIGHT;
		else if (toAdd == Vector3.down) _direction = PlayerDirection.DOWN;
		else _direction = PlayerDirection.UP;
	}

	#region TESTED
	// Verifica se pode mover para um POSIÇÃO no mapa, e não para um DIREÇÃO
	bool CanMoveTo (Vector3 position) {

		// Verificando se a posição de destino já não é a posição atual;
		if (_destination == transform.position) {
#if __DEBUG__
			Debug.LogWarning ("Trying to move for the current position!");
#endif
			return true;
		}
		
		// Verificando se as coordenadas do vetor são inteiros
		var sumF = position.x + position.y + position.z;
		var sumI = (int)sumF;
		if (sumI != sumF) {
			// As coordenadas não são inteiras
#if __DEBUG__
			Debug.LogError ("Invalid destination \"" + position + "\". Only can move to integer positions");
			UnityEditor.EditorApplication.isPlaying = false;
#endif
			return false;
		}


		// Verificando se a posição está fora do mapa
		// Lembrando que o pivô do player está no canto inf esq
		if (position.x < mapLimits.xMin ||
			(position.x + _tileSize) > mapLimits.xMax ||
			position.y < mapLimits.yMin ||
			(position.y - _tileSize) > mapLimits.yMax) {
#if __DEBUG__
			Debug.LogError ("The direction " + position + " move away of the map");
			UnityEditor.EditorApplication.isPlaying = false;
#endif
			return false;
		}


		// Verificando se o destino está na mesma linha ou na mesma coluna que a posição atual
		var diff = (position - transform.position).normalized;

		if (diff == Vector3.up) {
		} else if (diff == Vector3.down) {
		} else if (diff == Vector3.left) {
		} else if (diff == Vector3.right) {
		} else {
#if __DEBUG__
			Debug.LogError ("Invalid destination \"" + position + "\". Can only move horizontally or vertically");
			Debug.LogError ("Current position \"" + transform.position + "\".");
			UnityEditor.EditorApplication.isPlaying = false;
#endif
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
