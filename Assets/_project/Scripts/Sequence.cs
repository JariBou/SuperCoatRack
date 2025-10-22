using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using _project.ScriptableObjects.Scripts;
using _project.Scripts.Managers;

namespace _project.Scripts
{
    public class Sequence
    {
        private int _beatCounter;
        public List<SequenceData.SequenceAction> sequenceData = new();

        private Dictionary<int, SequenceData.SequenceAction> _sequences = new();

        public bool IsOver { get; private set; }

        public int BeatCounter => _beatCounter;
        public int MaxBeatCounter { get; private set; }

        private Sequence(SequenceData sequenceData)
        {
            _beatCounter = -1;
            foreach (SequenceData.SequenceAction action in sequenceData)
            {
                this.sequenceData.Add(action);
            }
            Initialize();
        }
        
        public void Initialize()
        {
            int beatCount = 0;
            //Debug.Log("Initializing Sequence...");
            foreach (SequenceData.SequenceAction action in sequenceData)
            {
                beatCount += action.BeatDelayToPrevious;
                //Debug.Log($"Action at beat {beatCount}");
                _sequences.Add(beatCount, action);
            }
            MaxBeatCounter = beatCount;
        }

        public Sequence DoBeat()
        {
            _beatCounter = BeatCounter + 1;
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
            int beatCounter = BeatCounter + peakCount;
            if (beatCounter >= MaxBeatCounter || beatCounter < 0)
            {
                result = null;
                return false;
            }
            return _sequences.TryGetValue(beatCounter, out result);
        }
        
        public bool PeakBeat(int peakCount, [MaybeNullWhen(false)] out GameAction result)
        {
            bool found = PeakBeat(peakCount, out SequenceData.SequenceAction resultAsSequence);
            result = resultAsSequence;
            return found;
        }

        public bool IsLastSequence(LevelSequencesManager.SequenceActionTimed lastSequenceAction)
        {
            SequenceData.SequenceAction sequenceAction = lastSequenceAction.SequenceAction;
            return sequenceData[^1] == sequenceAction;
        }
    }
}