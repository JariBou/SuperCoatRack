using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using _project.ScriptableObjects.Scripts;
using JetBrains.Annotations;
using UnityEngine;

namespace _project.Scripts
{
    public class Sequence
    {
        private int _beatCounter;
        public List<SequenceData.SequenceAction> sequenceData = new();

        private Dictionary<int, SequenceData.SequenceAction> _sequences = new();


        private Sequence(SequenceData sequenceData)
        {
            _beatCounter = 0;
            foreach (SequenceData.SequenceAction action in sequenceData)
            {
                this.sequenceData.Add(action);
            }
            Initialize();
        }
        
        public void Initialize()
        {
            int beatCount = 0;
            Debug.Log("Initializing Sequence...");
            foreach (SequenceData.SequenceAction action in sequenceData)
            {
                beatCount += action.BeatDelayToPrevious;
                Debug.Log($"Action at beat {beatCount}");
                _sequences.Add(beatCount, action);
            }
        }

        public Sequence DoBeat()
        {
            _beatCounter++;
            // Debug.Log($"Doing Beat {_beatCounter} of Sequence");
            return this;
        }

        public bool HasActionOnBeat([MaybeNullWhen(false)] out SequenceData.SequenceAction result)
        {
            return PeakBeat(0, out result);
        }

        public static Sequence FromSequenceData(SequenceData sequenceData)
        {
            return new Sequence(sequenceData);
        }
        
        public bool PeakBeat(int peakCount, [MaybeNullWhen(false)] out SequenceData.SequenceAction result)
        {
            return _sequences.TryGetValue(_beatCounter + peakCount, out result);
        }
    }
}