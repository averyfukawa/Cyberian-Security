using System;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioOcclusion : MonoBehaviour
{
    [Header("FMOD Event")]
    [SerializeField] [EventRef] private string selectAudio;
    private EventInstance audioInstance;
    private EventDescription audioDes;
    private StudioListener listening;
    private PLAYBACK_STATE pb;

    [Header("Occlusion Options")]
    [SerializeField, Range(0f, 10f)] private float soundOcclusionWidening = 1f;
    [SerializeField, Range(0f, 10f)] private float playerOcclusionWidening = 1f;
    [SerializeField] private LayerMask occlusionLayer;

    public bool playsFromStart = true;

    private bool audioIsVirtual;
    private float maxDistance;
    private float listenerDistance;
    private float lineCastHitCount = 0f;
    private Color colour;

    [HideInInspector] public bool isPlaying = false;
    private void Update()
    {
        if (playsFromStart)
        {
            if (!isPlaying)
            {
                isPlaying = true;
                audioInstance = RuntimeManager.CreateInstance(selectAudio);
                RuntimeManager.AttachInstanceToGameObject(audioInstance, GetComponent<Transform>(),
                    GetComponent<Rigidbody>());
                audioInstance.start();
                audioInstance.release();

                audioDes = RuntimeManager.GetEventDescription(selectAudio);
                audioDes.getMaximumDistance(out maxDistance);

                listening = FindObjectOfType<StudioListener>();   
            }
        }

    }

    private void FixedUpdate()
    {        
        audioInstance.isVirtual(out audioIsVirtual);
        audioInstance.getPlaybackState(out pb);
        listenerDistance = Vector3.Distance(transform.position, listening.transform.position);

        if (!audioIsVirtual && pb == PLAYBACK_STATE.PLAYING && listenerDistance <= maxDistance)
            OccludeBetween(transform.position, listening.transform.position);

        lineCastHitCount = 0f;
    }

    private void OccludeBetween(Vector3 sound, Vector3 listener)
    {
        Vector3 soundLeft = CalculatePoint(sound, listener, soundOcclusionWidening, true);
        Vector3 soundRight = CalculatePoint(sound, listener, soundOcclusionWidening, false);

        //Vector3 soundAbove = new Vector3(sound.x, sound.y + soundOcclusionWidening, sound.z);
        //Vector3 soundBelow = new Vector3(sound.x, sound.y - soundOcclusionWidening, sound.z);

        Vector3 listenerLeft = CalculatePoint(listener, sound, playerOcclusionWidening, true);
        Vector3 listenerRight = CalculatePoint(listener, sound, playerOcclusionWidening, false);

        //Vector3 listenerAbove = new Vector3(listener.x, listener.y + playerOcclusionWidening * 0.5f, listener.z);
        //Vector3 listenerBelow = new Vector3(listener.x, listener.y - playerOcclusionWidening * 0.5f, listener.z);

        CastLine(soundLeft, listenerLeft);
        CastLine(soundLeft, listener);
        CastLine(soundLeft, listenerRight);

        CastLine(sound, listenerLeft);
        CastLine(sound, listener);
        CastLine(sound, listenerRight);

        CastLine(soundRight, listenerLeft);
        CastLine(soundRight, listener);
        CastLine(soundRight, listenerRight);
        
        //CastLine(soundAbove, listenerAbove);
        //CastLine(soundBelow, listenerBelow);

        if (playerOcclusionWidening == 0f || soundOcclusionWidening == 0f)
        {
            colour = Color.blue;
        }
        else
        {
            colour = Color.green;
        }

        SetParameter();
    }

    private Vector3 CalculatePoint(Vector3 a, Vector3 b, float m, bool posOrneg)
    {
        float x;
        float z;
        float n = Vector3.Distance(new Vector3(a.x, 0f, a.z), new Vector3(b.x, 0f, b.z));
        float mn = (m / n);
        if (posOrneg)
        {
            x = a.x + (mn * (a.z - b.z));
            z = a.z - (mn * (a.x - b.x));
        }
        else
        {
            x = a.x - (mn * (a.z - b.z));
            z = a.z + (mn * (a.x - b.x));
        }
        return new Vector3(x, a.y, z);
    }

    private void CastLine(Vector3 start, Vector3 end)
    {
        RaycastHit hit;
        Physics.Linecast(start, end, out hit, occlusionLayer);

        if (hit.collider)
        {
            lineCastHitCount++;
            Debug.DrawLine(start, end, Color.red);
        }
        else
            Debug.DrawLine(start, end, colour);
    }

    private void SetParameter()
    {
        audioInstance.setParameterByName("Occlusion", lineCastHitCount / 11);
    }
}