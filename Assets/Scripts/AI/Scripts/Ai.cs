using UnityEngine;

namespace Ai.Scripts {

    [System.Serializable]
    public class Ai : MonoBehaviour {

        #region Editor Variables

        // ***** EDITOR VARIABLES ***** //
        public bool _CanFollowPlayer,
            _CanFollowBreadcrumbs,
            _CanFollowAi,
            _CanWander,
            _CanPatrol,
            _CanWanderAnywhere,
            _CanHover,
            _CanFlee,
            _CanJump,
            _CanLongJump,
            _IsJumping,
            _HasAvoidance,
            _HasEdgeAvoidance,
            _HasVision,
            _HasFrontVision,
            _IsMelee,
            _IsRanged,
            _IsGround,
            _IsAir,
            _IsInvincible;

        public float followSpeed,
            wanderSpeed,
            patrolSpeed,
            rotationSpeed,
            avoidSpeed,
            jumpDistance,
            jumpForce,
            longJumpForce,
            followDistance,
            wanderDistance,
            attackDistance,
            visionDistance,
            avoidDistance,
            edgeDistance,
            otherAiDistance,
            wanderTimeLimit,
            wanderTimeRate,
            hoverHeight,
            hoverForce,
            Health;

        #endregion

        #region STATES

        public enum LifeState {

            IsAlive,
            IsDead,
            IsInvincible

        }

        public LifeState lifeState = LifeState.IsAlive;

        public enum VisionState {

            CanSeeNothing,
            CanSeePlayer,
            CanSeeBreadcrumb,
            CanSeeFollowAi

        }

        public VisionState visionState = VisionState.CanSeeNothing;

        public enum MovementState {

            IsIdle,
            IsFollowingPlayer,
            IsWandering

        }

        public MovementState moveState = MovementState.IsIdle;

        public enum AttackState {

            CanNotAttack,
            CanAttackPlayer

        }

        public AttackState attackState = AttackState.CanNotAttack;

        #endregion

        // GAMEOBJECTS
        [HideInInspector] public Transform Player, // Targeted Player
            Hover, // Hover Start Pos
            Edge, // Edge Avoidance Start Pos
            LongJumpDetector, // Jump to start Pos
            JumpDetector; // Jump detector

        // LAYERS
        [HideInInspector] public LayerMask playerLayer, // Layer : Player
            enemyLayer, // Layer : Enemy
            breadcrumbLayer, // Layer : Breadcrumb
            waypointLayer; // Layer : Waypoint

        // PRIVATE VARIABLES
        private bool _isWandering;
        private bool _isAvoiding; // Used for avoidance, removes velocity after avoidance
        private bool _hasWanderPos;
        private Vector3 _currentWanderPos;
        private Vector3 _wanderPos; // Sets next random wander position
        private float _wanderTimer, _wanderNext; // Used for timing the wander time limit
        private RaycastHit _hit;

        private void Start() {
            StartCoroutine(AiManager.Ai_Lists());
            StartCoroutine(this.Ai_Layers());
        }

        private void Update() {
            Ai_LifeState();
            if (IsGrounded()) {
                _IsJumping = false;
            }
        }

        private void FixedUpdate() {
            Ai_Controller(); // Controls Ai Movement & Attack States
            Ai_Avoidance(~(breadcrumbLayer | enemyLayer | playerLayer | waypointLayer)); // Controls Ai wall avoidance
        }

        private void Ai_Controller() {
            // Checks if following player is enabled and a player has been found	
            if (_CanFollowPlayer && this.Ai_FindPlayer()) {
                _hasWanderPos = false;
                visionState = VisionState.CanSeePlayer;

                if (_IsRanged) {
                    // Is this a ranged ground unit?
                    if (Vector3.Distance(transform.position, Player.position) > followDistance) {
                        moveState = MovementState.IsFollowingPlayer;
                        attackState = AttackState.CanNotAttack;
                        Ai_Movement(Player.position, followSpeed);
                    }
                    else if (_CanFlee && Vector3.Distance(transform.position, Player.position) <= attackDistance) {
                        moveState = MovementState.IsFollowingPlayer;
                        attackState = AttackState.CanNotAttack;
                        Ai_Flee();
                    }
                    else {
                        moveState = MovementState.IsIdle;
                        attackState = AttackState.CanAttackPlayer;
                        Ai_Rotation(Player.position);
                    }
                }
                else if (_IsMelee) {
                    // Is this a melee ground unit?
                    if (Vector3.Distance(transform.position, Player.position) > followDistance) {
                        moveState = MovementState.IsFollowingPlayer;
                        attackState = AttackState.CanNotAttack;
                        Ai_Movement(Player.position, followSpeed);
                    }
                    else if (Vector3.Distance(transform.position, Player.position) <= attackDistance) {
                        moveState = MovementState.IsIdle;
                        attackState = AttackState.CanAttackPlayer;
                        Ai_Rotation(Player.position);
                    }
                }

                Debug.DrawLine(transform.position, Player.position, Color.red);
            }
            else if (_CanWander && _wanderTimer < wanderTimeLimit) {
                visionState = VisionState.CanSeeNothing;
                attackState = AttackState.CanNotAttack;
                Ai_Wander();
            }
            else {
                Ai_Reset();
            }
        }

        private void Ai_Reset() {
            Player = null;
            _wanderTimer = 0;
            moveState = MovementState.IsIdle;
            attackState = AttackState.CanNotAttack;
        }


        // Move the rigidbody forward based on the speed value 
        private void Ai_Movement(Vector3 position, float speed) {
            if (Ai_EdgeAvoidance() && !_IsJumping) {
                GetComponent<Rigidbody>()
                    .MovePosition(GetComponent<Rigidbody>().position + transform.forward * (Time.deltaTime * speed));
            }

            Ai_Rotation(position);
        }

        // Rotate the Ai to look towards it's target at a set Rotation speed
        private void Ai_Rotation(Vector3 position) {
            var playerPos = Vector3.zero;
            
            if (_IsGround) {
                playerPos = new Vector3(position.x, transform.position.y,
                    position.z); // Adjust Y position so Ai doesn't rotate up/down
            }
            else if (_IsAir) {
                playerPos = new Vector3(position.x, position.y, position.z);
            }

            var transform1 = transform;
            GetComponent<Rigidbody>().MoveRotation(Quaternion.Slerp(transform1.rotation,
                Quaternion.LookRotation(playerPos - transform1.position), rotationSpeed));
        }

        private void Ai_Flee() {
            var transform1 = transform;
            GetComponent<Rigidbody>().MoveRotation(Quaternion.Slerp(transform1.rotation,
                Quaternion.LookRotation(transform1.position - Player.position), rotationSpeed));
            if (Ai_EdgeAvoidance()) {
                GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position +
                                                       transform.forward * (Time.deltaTime * followSpeed));
            }
        }

        // This wander function selects a random location around the Ai and moves towards it.
        // This will be update in the future to allow specific wander radius rather than "anywhere"		
        private void Ai_Wander() {
            _wanderTimer += Time.deltaTime;
            _isWandering = !(_wanderTimer >= wanderTimeLimit);

            if (_CanWanderAnywhere) {
                _currentWanderPos = transform.position;
            }
            else {
                if (!_hasWanderPos) {
                    _currentWanderPos = transform.position;
                    _hasWanderPos = true;
                }
            }

            if (!_isWandering) return;
            
            if (Time.time > _wanderNext) {
                _wanderNext = Time.time + wanderTimeRate;
                var wanderX = Random.Range(_currentWanderPos.x - wanderDistance,
                    _currentWanderPos.x + wanderDistance);
                var wanderZ = Random.Range(_currentWanderPos.z - wanderDistance,
                    _currentWanderPos.z + wanderDistance);
                _wanderPos = new Vector3(wanderX, _currentWanderPos.y, wanderZ);
            }

            if (Vector3.Distance(transform.position, _wanderPos) > 0.5f) {
                Ai_Movement(_wanderPos, wanderSpeed);
                moveState = MovementState.IsWandering;
            }
            else {
                moveState = MovementState.IsIdle;
            }
        }

        // Avoidance casts a ray around the Ai so that it can bounce of walls and other obstacles
        // Velocity is set to zero so that when the AddForce is no longer being applied it will stop the Ai from sliding around
        private void Ai_Avoidance(LayerMask layer) {
            if (!_HasAvoidance) return;
            
            if (Physics.Raycast(transform.position, -transform.right, out _hit, avoidDistance, layer)) {
                Debug.DrawLine(transform.position, _hit.point, Color.cyan);
                GetComponent<Rigidbody>().AddForce(transform.right * avoidSpeed);
                _isAvoiding = true;
            }

            if (Physics.Raycast(transform.position, transform.right, out _hit, avoidDistance, layer)) {
                Debug.DrawLine(transform.position, _hit.point, Color.cyan);
                GetComponent<Rigidbody>().AddForce(-transform.right * avoidSpeed);
                _isAvoiding = true;
            }

            if (Physics.Raycast(transform.position, transform.forward + -transform.right * 2, out _hit,
                avoidDistance, layer)) {
                Debug.DrawLine(transform.position, _hit.point, Color.cyan);
                GetComponent<Rigidbody>().AddForce(transform.right * avoidSpeed);
                _isAvoiding = true;
            }

            if (Physics.Raycast(transform.position, transform.forward + transform.right * 2, out _hit,
                avoidDistance, layer)) {
                Debug.DrawLine(transform.position, _hit.point, Color.cyan);
                GetComponent<Rigidbody>().AddForce(-transform.right * avoidSpeed);
                _isAvoiding = true;
            }

            if (Physics.Raycast(transform.position, -transform.forward, out _hit, avoidDistance, layer)) {
                Debug.DrawLine(transform.position, _hit.point, Color.cyan);
                GetComponent<Rigidbody>().AddForce(transform.forward * avoidSpeed);
                _isAvoiding = true;
            }

            // This raycast helps avoid other Ai that are directly infront
            if (Physics.Raycast(transform.position, transform.forward, out _hit,
                transform.GetComponent<Collider>().bounds.extents.z + 0.1f)) {
                if (_hit.collider.CompareTag(Constants.EnemyTag)) {
                    GetComponent<Rigidbody>().AddForce(transform.right * avoidSpeed);
                    _isAvoiding = true;
                }
            }

            if (!_isAvoiding) return;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            _isAvoiding = false;
        }

        // Self Harm Avoidance casts a ray to see if there's ground infront of the Ai, if there's no ground return false
        private bool Ai_EdgeAvoidance() {
            if (!_HasEdgeAvoidance || !_HasAvoidance) return true;
            var position = Edge.position;
            var up = Edge.up;
            Debug.DrawLine(position, position + -up * edgeDistance);
            return Physics.Raycast(position, -up, edgeDistance);
        }

        // We simply check to see if this Ai is invincible, if so then the lifestate is set to IsInvincible.
        // Otherwise check to see if the Health is equal or lower to 0 before setting to IsDead state.
        private void Ai_LifeState() {
            if (_IsInvincible) {
                lifeState = LifeState.IsInvincible;
            }
            else {
                if (Health <= 0.0f) {
                    lifeState = LifeState.IsDead;
                }
            }
        }

        // Checks if a position is within range if vision is enabled
        public bool InRange(Vector3 position, float vision) {
            if (!_HasVision) return true;
            if (!(Vector3.Distance(transform.position, position) < vision)) return false;
            if (!_HasFrontVision) return true;
            var transform1 = transform;
            var visionAngle = Vector3.Dot(position - transform1.position, transform1.forward);
            return visionAngle > 0;
        }

        // This checks if the Ai is grounded, collider is required on the GameObject that has this script
        public bool IsGrounded() {
            return Physics.Raycast(
                transform.position, 
                -Vector3.up, 
                GetComponent<Collider>().bounds.extents.y + 0.1f);
        }

    }

}