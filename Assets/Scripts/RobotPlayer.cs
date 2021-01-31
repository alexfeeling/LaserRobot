using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Robot
{

    public struct PlayerData
    {

        public bool isRedLaserOpen;

        public float AddedLaserLangth;

        public float MoveSpeed;

        public bool LockJump;
    }

    public class RobotPlayer : MonoBehaviour
    {

        public enum LaserColor
        {
            Blue,//蓝光反射
            Red,//红光推动
        }

        public Transform P_Raser;

        public AudioSource moveAudioSource;

        public LayerMask layerGround;
        public LayerMask layerRaserHit;

        public LaserColor laserColor;

        public Transform RaserShooter;
        public Material RaserMat;

        public Color BlueColor;
        public Color RedColor;

        public LineRenderer RaserLine;

        public PlayerData playerData = new PlayerData();
        public PlayerData lastPlayerData;

        public bool IsFreeRotate;

        public bool IsFreeMove;

        [Header("移动速度")]
        public float DefaultMoveSpeed = 2f;

        public RobotCharacterController Character;

        public float LaserSpeed = 40f;

        //public bool isRedLaserOpen = false;

        public float LimitMaxRaserLength = 20f;
        //public float AddedLaserLangth = 0f;

        private void Start()
        {
            //ResetPlayer();
        }

        public void SetPlayerDefault()
        {
            lastPlayerData = new PlayerData()
            {
                isRedLaserOpen = false,
                MoveSpeed = this.DefaultMoveSpeed,
                LockJump = true,
                AddedLaserLangth = 0,
            };
            playerData = lastPlayerData;
        }

        public void ResetPlayer()
        {
            playerData = lastPlayerData;
            //isRedLaserOpen = false;
            Character.MaxStableMoveSpeed = playerData.MoveSpeed;
            Character.LockJump = true;
            //AddedLaserLangth = 0f;

            laserColor = LaserColor.Blue;
            RaserMat.SetColor("_Color", BlueColor * 4);
            RaserLine.endWidth = 0.1f;
        }

        public void RecordPlayerData()
        {
            lastPlayerData = playerData;
        }

        private void ShowRedLaserText()
        {
            UIPlayerMainCon.getInstance().playText(3);
        }
        public void AddProp(Gear gear)
        {
            if (gear.propType == Gear.PropType.RedLaser)
            {
                playerData.isRedLaserOpen = true;

                Text3DAdder.getInstance().addText("获得红色激光能力", new Color(1, 0.1f, 0.1f));

                Invoke("ShowRedLaserText", 1f);
            }
            else if (gear.propType == Gear.PropType.LaserLengthen)
            {
                playerData.AddedLaserLangth += 20;
                Text3DAdder.getInstance().addText("射线距离加长", new Color(0, 0.7f, 1f));
            }
            else if (gear.propType == Gear.PropType.Movable)
            {
                playerData.MoveSpeed += 3;
                Character.MaxStableMoveSpeed = playerData.MoveSpeed;
                Text3DAdder.getInstance().addText("移动速度加快", new Color(1, 0.7f, 0f));
            }
            else if (gear.propType == Gear.PropType.Jump)
            {
                Character.LockJump = false;
                Text3DAdder.getInstance().addText("获得跳跃能力", new Color(0, 1f, 0.1f));
            }

            AudioManager.Insatnce.PlaySound(AudioManager.Insatnce.Effect_AddGear);
        }

        void Update()
        {
            if (GameManager.Instance.Status != GameManager.GameStatus.Playing) return;

            if (UIPlayerMainCon.getInstance().IsOnPlay) return;

            UpdateDirection();
            //UpdateMove();
            HandleCharacterInput();

            UpdateRaserControll();

        }

        private const string HorizontalInput = "Horizontal";
        private const string VerticalInput = "Vertical";

        private void HandleCharacterInput()
        {
            var characterInputs = new RobotCharacterInputs();

            // Build the CharacterInputs struct
            characterInputs.MoveAxisForward = Input.GetAxisRaw(VerticalInput);
            characterInputs.MoveAxisRight = Input.GetAxisRaw(HorizontalInput);
            characterInputs.JumpDown = Input.GetKeyDown(KeyCode.Space);
            //characterInputs.CrouchDown = Input.GetKeyDown(KeyCode.C);
            //characterInputs.CrouchUp = Input.GetKeyUp(KeyCode.C);

            // Apply inputs to character
            Character.SetInputs(ref characterInputs);

            if (Character.Motor.Velocity.sqrMagnitude > 0)
            {

            }
            else
            {

            }
        }

        private void PlayMoveSound(bool isMoving)
        {
            if (isMoving)
            {
                if (!_isMoving)
                {
                    _isMoving = true;
                    moveAudioSource.Play();
                }
            }
            else
            {
                if (!_isMoving)
                {
                    _isMoving = false;
                    moveAudioSource.Stop();
                }
            }
        }

        private const int MAX_MOVE_COMMAND_COUNT = 5;
        private bool _isShooting;
        private bool _isMoving = false;
        private List<Vector2> _moveCmdList = new List<Vector2>();
        private void UpdateMove()
        {
            if (_isShooting) return;


            if (IsFreeMove)
            {
                FreeMove();
            }
            else
            {
                var isUp = Input.GetKeyDown(KeyCode.W);
                var isDown = Input.GetKeyDown(KeyCode.S);
                var isLeft = Input.GetKeyDown(KeyCode.A);
                var isRight = Input.GetKeyDown(KeyCode.D);
                if (isUp)
                    Move(Vector2.up);
                else if (isDown)
                    Move(Vector2.down);
                else if (isLeft)
                    Move(Vector2.left);
                else if (isRight)
                    Move(Vector2.right);
            }
        }

        private void FreeMove()
        {
            var isUp = Input.GetKey(KeyCode.W);
            var isDown = Input.GetKey(KeyCode.S);
            var isLeft = Input.GetKey(KeyCode.A);
            var isRight = Input.GetKey(KeyCode.D);
            var dir = Vector3.zero;
            _isMoving = true;
            if (isUp)
            {
                dir.z = 1;
                if (isLeft)
                {
                    dir.x = -1;
                    dir.Normalize();
                }
                else if (isRight)
                {
                    dir.x = 1;
                    dir.Normalize();
                }
                else
                {

                }
            }
            else if (isDown)
            {
                dir.z = -1;
                if (isLeft)
                {
                    dir.x = -1;
                    dir.Normalize();
                }
                else if (isRight)
                {
                    dir.x = 1;
                    dir.Normalize();
                }
                else
                {

                }
            }
            else
            {
                if (isLeft)
                {
                    dir.x = -1;
                    dir.Normalize();
                }
                else if (isRight)
                {
                    dir.x = 1;
                    dir.Normalize();
                }
                else
                {
                    _isMoving = false;
                }
            }
            if (_isMoving)
            {
                transform.position += dir * DefaultMoveSpeed * Time.deltaTime;
            }
        }

        private void Move(Vector2 dir)
        {
            if (_isMoving)
            {
                if (_moveCmdList.Count < MAX_MOVE_COMMAND_COUNT)
                    _moveCmdList.Add(dir);
                return;
            }
            _isMoving = true;
            var desPos = transform.position + new Vector3(dir.x, 0, dir.y) * 1;
            transform.DOMove(desPos, 0.2f).SetEase(Ease.OutCubic).OnComplete(NextMove);
        }

        private void NextMove()
        {
            if (_moveCmdList.Count <= 0)
            {
                _isMoving = false;
                return;
            }

            var dir = _moveCmdList[0];
            _moveCmdList.RemoveAt(0);
            _isMoving = false;
            Move(dir);
        }

        private int _lastAngle = 0;
        private bool _isRotating = false;
        private void UpdateDirection()
        {
            if (_isShooting) return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, layerGround, QueryTriggerInteraction.Ignore))
            {
                var dir = hit.point - transform.position;
                dir.y = 0;
                var qua = Quaternion.LookRotation(dir);
                //var angle = Mathf.RoundToInt(qua.eulerAngles.y / 45f) * 45;
                if (IsFreeRotate)
                {
                    transform.rotation = qua;
                }
                else
                {
                    var angle = qua.eulerAngles.y;
                    angle = Mathf.RoundToInt(qua.eulerAngles.y / 90f) * 90;
                    if (angle != _lastAngle)
                    {
                        _isRotating = true;
                        transform.DORotate(new Vector3(0, angle, 0),
                            0.2f).SetEase(Ease.OutCubic).OnComplete(() => _isRotating = false);
                        _lastAngle = Mathf.RoundToInt(angle);
                    }
                }
            }
        }

        //private void OnGUI()
        //{
        //    var angle = transform.rotation.eulerAngles.y;
        //    GUI.Label(new Rect(100, 100, 100, 20), "Angle:" + angle.ToString("#.##"));
        //}
        private float _lastSwitchTime = 0;
        private void UpdateRaserControll()
        {
            if (!_isShooting)
            {
                //右键切换激光颜色
                if (playerData.isRedLaserOpen)
                {
                    if (Input.GetMouseButtonDown(1))
                    {
                        var time = Time.time;
                        if (time - _lastSwitchTime < 0.5f)
                        {
                            return;
                        }
                        _lastSwitchTime = time;
                        AudioManager.Insatnce.PlaySound(AudioManager.Insatnce.Effect_SwitchLaser);
                        if (laserColor == LaserColor.Blue)
                        {
                            laserColor = LaserColor.Red;
                            RaserMat.SetColor("_Color", RedColor * 4);
                            RaserLine.endWidth = 0.3f;
                        }
                        else
                        {
                            laserColor = LaserColor.Blue;
                            RaserMat.SetColor("_Color", BlueColor * 4);
                            RaserLine.endWidth = 0.1f;
                        }
                    }
                }
                else if (laserColor == LaserColor.Red)
                {
                    laserColor = LaserColor.Blue;
                    RaserMat.SetColor("_Color", BlueColor * 4);
                    RaserLine.endWidth = 0.1f;
                }
            }

            //if (_isMoving || _isRotating) return;

            //左键发射激光
            if (Input.GetMouseButtonDown(0))
            {
                //ShootRaser(RaserShooter.position, transform.forward, 0);
                //StartCoroutine(CorShootRaseLine(RaserShooter.position, transform.forward, 0));
                _maxRaserLength = 0.1f;
                if (laserColor == LaserColor.Blue)
                {
                    AudioManager.Insatnce.PlaySound(AudioManager.Insatnce.Effect_LaserBlue);
                }
                else
                {
                    AudioManager.Insatnce.PlaySound(AudioManager.Insatnce.Effect_LaserRed);
                }
            }
            if (Input.GetMouseButton(0))
            {
                if (_maxRaserLength > 0)
                {
                    _maxRaserLength = Mathf.Min(LimitMaxRaserLength + playerData.AddedLaserLangth,
                        _maxRaserLength + LaserSpeed * Time.deltaTime);
                    ShootRaseLine(RaserShooter.position, transform.forward, 0, 0);
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _maxRaserLength = 0;
                DestroyRasers();
            }
        }

        private float _maxRaserLength = 0;
        private void ShootRaseLine(Vector3 startPos, Vector3 lookDir, float distance, int index)
        {

            Ray ray = new Ray(startPos, lookDir);
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, layerRaserHit, QueryTriggerInteraction.Ignore))
            {
                RaserLine.positionCount = index + 2;
                RaserLine.SetPosition(index, startPos);

                var desPot = hit.point;
                var length = (desPot - startPos).magnitude;
                if (distance + length > _maxRaserLength)
                {
                    length = _maxRaserLength - distance;
                    RaserLine.SetPosition(index + 1, startPos + lookDir * length);
                }
                else
                {

                    RaserLine.SetPosition(index + 1, startPos + lookDir * length);

                    var go = hit.collider.gameObject;
                    if (go.CompareTag("Gear"))
                    {
                        var gear = go.GetComponent<Gear>();
                        gear.HitByRaser();
                    }
                    else if (go.CompareTag("Block") || go.CompareTag("Pushable"))
                    {
                        var inv = lookDir.normalized;
                        var nor = hit.normal.normalized;
                        //R = I - 2.0 * dot(N, I) * N;
                        var r = inv - 2f * Vector3.Dot(nor, inv) * nor;

                        if (laserColor == LaserColor.Blue)
                        {
                            ShootRaseLine(desPot, r, distance + length, index + 1);
                        }
                        else if (go.CompareTag("Pushable"))
                        {
                            var box = go.GetComponent<Box>();
                            box.BePush(transform.position);
                        }
                    }
                    else if (go.CompareTag("Player"))
                    {
                        BeAttack();
                    }
                    else if (go.CompareTag("Switch"))
                    {
                        var st = go.GetComponent<SwitchTower>();
                        st.Switch();
                    }

                }

            }
            else
            {
                RaserLine.positionCount = index + 2;
                RaserLine.SetPosition(index, startPos);

                var desPot = hit.point;
                var length = _maxRaserLength - distance;
                RaserLine.SetPosition(index + 1, startPos + lookDir * length);

            }
        }

        public void BeAttack()
        {
            AudioManager.Insatnce.PlaySound(AudioManager.Insatnce.Effect_Explode);
            GameManager.Instance.GameOver();
        }



        //private IEnumerator CorShootRaseLine(Vector3 startPos, Vector3 lookDir, int index)
        //{
        //    if (index >= 10) yield break;

        //    Ray ray = new Ray(startPos, lookDir);
        //    if (Physics.Raycast(ray, out var hit, Mathf.Infinity, layerObject, QueryTriggerInteraction.Ignore))
        //    {
        //        RaserLine.positionCount = index + 2;
        //        RaserLine.SetPosition(index, startPos);

        //        var desPot = hit.point;
        //        var length = (desPot - startPos).magnitude;
        //        var time = length / _raserSpeed;
        //        var cur = 0f;
        //        while (cur < time)
        //        {
        //            cur += Time.deltaTime;
        //            var rate = cur / time;
        //            var curPos = startPos + lookDir * length * rate;
        //            RaserLine.SetPosition(index + 1, curPos);
        //            yield return null;
        //        }

        //        RaserLine.SetPosition(index + 1, startPos + lookDir * length);

        //        var inv = lookDir.normalized;
        //        var nor = hit.normal.normalized;
        //        //R = I - 2.0 * dot(N, I) * N;
        //        var r = inv - 2f * Vector3.Dot(nor, inv) * nor;

        //        StartCoroutine(CorShootRaseLine(desPot, r, index + 1));
        //    }
        //    else
        //    {
        //        RaserLine.positionCount = index + 2;
        //        RaserLine.SetPosition(index, startPos);

        //        var desPot = hit.point;
        //        var length = 5f;
        //        var time = length / _raserSpeed;
        //        var cur = 0f;
        //        while (cur < time)
        //        {
        //            cur += Time.deltaTime;
        //            var rate = cur / time;
        //            var curPos = startPos + lookDir * length * rate;
        //            RaserLine.SetPosition(index + 1, curPos);
        //            yield return null;
        //        }

        //        RaserLine.SetPosition(index + 1, startPos + lookDir * length);
        //    }
        //}

        //private List<Transform> _raseList = new List<Transform>();
        //private void ShootRaser(Vector3 startPos, Vector3 lookDir, int index)
        //{
        //    if (index >= 5) return;
        //    _isShooting = true;

        //    Ray ray = new Ray(startPos, lookDir);

        //    if (Physics.Raycast(ray, out var hit, Mathf.Infinity, layerObject, QueryTriggerInteraction.Ignore))
        //    {
        //        var length = (hit.point - startPos).magnitude;
        //        var raser = Instantiate(P_Raser);
        //        raser.position = startPos;
        //        raser.rotation = Quaternion.LookRotation(lookDir);
        //        raser.localScale = new Vector3(1, 1, 0);
        //        _raseList.Add(raser);
        //        raser.DOScaleZ(length, length / _raserSpeed).OnComplete(() =>
        //         {
        //             if (hit.collider.CompareTag("Gear"))
        //             {
        //                 var gear = hit.collider.gameObject;
        //                 Destroy(gear);
        //                 //DestroyRasers();
        //             }
        //             else if (hit.collider.CompareTag("Block"))
        //             {
        //                 var inv = lookDir.normalized;
        //                 var nor = hit.normal.normalized;
        //                 //R = I - 2.0 * dot(N, I) * N;
        //                 var r = inv - 2f * Vector3.Dot(nor, inv) * nor;

        //                 ShootRaser(hit.point, r, index + 1);
        //             }
        //         });
        //    }
        //    else
        //    {
        //        var length = 5;
        //        var raser = Instantiate(P_Raser);
        //        raser.position = startPos;
        //        raser.rotation = Quaternion.LookRotation(lookDir);
        //        raser.localScale = new Vector3(1, 1, 0);
        //        raser.DOScaleZ(length, length / _raserSpeed);
        //        _raseList.Add(raser);
        //    }
        //}

        private void DestroyRasers()
        {
            _isShooting = false;
            //for (int i = 0; i < _raseList.Count; i++)
            //{
            //    _raseList[i].DOKill();
            //    Destroy(_raseList[i].gameObject);
            //}
            //_raseList.Clear();

            StopAllCoroutines();
            RaserLine.positionCount = 0;
        }

    }
}