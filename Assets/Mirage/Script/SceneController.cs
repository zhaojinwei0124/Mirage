using UnityEngine;
using System.Collections;

namespace Mirage
{
    public class SceneController : MonoBehaviour
    {
        [SerializeField] float _leadTime = 1;

        [Space]

        [SerializeField, Range(0, 1)] float _pointLightIntensity;
        [SerializeField, Range(0, 1)] float _frontLightIntensity;
        [SerializeField, Range(0, 1)] float _spotLightIntensity;
        [SerializeField, Range(0, 1)] float _spotLightAngle;
        [SerializeField, Range(0, 1)] float _lightEnvelope;

        [Space]

        [SerializeField, Range(0, 1)] float _swarm1Throttle;
        [SerializeField, Range(0, 1)] float _swarm2Throttle;
        [SerializeField, Range(0, 1)] float _swarm3Throttle;

        [Space]

        [SerializeField, Range(0, 1)] float _dustThrottle;
        [SerializeField, Range(0, 1)] float _shardsThrottle;
        [SerializeField, Range(0, 1)] float _rocksThrottle;

        [Space]

        [SerializeField] Light _pointLight;
        [SerializeField] Transform _pointLightSphere;
        [SerializeField] Light _frontLight;
        [SerializeField] Light[] _spotLights;
        [SerializeField] Transform[] _spotLightPivots;

        [Space]

        [SerializeField] Kvant.Swarm _swarm1;
        [SerializeField] Kvant.Swarm _swarm2;
        [SerializeField] Kvant.Swarm _swarm3;

        [Space]

        [SerializeField] Kvant.Spray _dust;
        [SerializeField] Kvant.Spray _shards;
        [SerializeField] Kvant.Spray _rocks;

        const float _beatInterval = 60.0f / 140 * 2;

        public void ResetSwarm(int index)
        {
            if (index == 0) _swarm1.Restart();
            if (index == 1) _swarm2.Restart();
            if (index == 2) _swarm3.Restart();
        }

        IEnumerator Start()
        {
            yield return new WaitForSeconds(_leadTime);
            GetComponent<Animator>().Play("Main");
        }

        void Update()
        {
            var env = 1.0f - (Time.time - _leadTime) / _beatInterval % 1.0f;
            env *= _lightEnvelope * 0.4f;

            var sphere = _pointLightIntensity + env;
            sphere *= 1.0f + 0.1f * Mathf.Sin(Time.time * 19);

            _pointLight.intensity = (_pointLightIntensity + env) * 1.8f;
            _pointLightSphere.localScale = Vector3.one * sphere * 0.1f;

            _frontLight.intensity = (_frontLightIntensity + env) * 0.9f;

            foreach (var l in _spotLights)
                l.intensity = (_spotLightIntensity + env) * 2.0f;

            var angle = Mathf.Lerp(-5.0f, 60.0f, _spotLightAngle);
            _spotLightPivots[0].rotation = Quaternion.Euler(angle, -9, 0);
            _spotLightPivots[1].rotation = Quaternion.Euler(angle, +9, 0);

            _swarm1.throttle = _swarm1Throttle;
            _swarm2.throttle = _swarm2Throttle;
            _swarm3.throttle = _swarm3Throttle;

            _dust.throttle = _dustThrottle;
            _shards.throttle = _shardsThrottle;
            _rocks.throttle = _rocksThrottle;
        }
    }
}
