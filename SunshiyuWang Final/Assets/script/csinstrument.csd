<Cabbage>
form caption("Sine Pluck Synth") size(400, 300), guiMode("queue"), pluginId("def1")
keyboard bounds(8, 158, 381, 95)
rslider bounds(10, 10, 20, 100), range(0, 1, 1), channel("volume"), text("Volume")
combobox bounds(40, 10, 100, 20), channel("waveform"), text("Waveform"), value(1), items("Sine", "Saw", "Square", "Triangle")
button bounds(150, 10, 100, 40), channel("lfoBypass"), text("LFO: Active", "LFO: Bypass "), value(0), latched(1)
</Cabbage>

<CsoundSynthesizer>
<CsOptions>
-odac -d -+rtmidi=NULL -M0 --midi-key-cps=4 --midi-velocity-amp=5
</CsOptions>
<CsInstruments>
; Initialize the global variables
ksmps = 32
nchnls = 2
0dbfs = 1

; Instrument to play a pluck sound
instr Pluck
  ; Use p4 for frequency and p5 for amplitude
  ifreq = p4
  iamp = p5

  ; Get the volume from the "volume" channel
  kVolume chnget "volume"

  ; Get the waveform selection from the combobox
  iWaveform chnget "waveform"

  ; Get the LFO bypass state from the button
  kLfoBypass chnget "lfoBypass"
  prints "LFO Bypass State: %d\n", kLfoBypass, 0.1

  ; Sine LFO for volume modulation
  kLfoDepth = 0.5           ; Depth of LFO modulation
  kLfoRate = 10              ; Frequency of LFO in Hz
  kLfo oscili kLfoDepth, kLfoRate, 1 ; Sine LFO

  ; Apply LFO modulation only if bypass is not active
  kModulation = (1 - kLfoBypass) * kLfo

  ; Simple ADSR envelope
  kEnv madsr 0.01, 0.1, 0.2, 0.5

  ; Oscillator with pluck effect and LFO modulation
  aOut oscili iamp * (kVolume + kModulation) * kEnv, ifreq, iWaveform

  ; Output to both stereo channels
  outs aOut, aOut
endin

</CsInstruments>
<CsScore>
; Define function tables for different waveforms
f 1 0 16384 10 1            ; Sine wave
f 2 0 16384 7 1 16384 0     ; Sawtooth wave
f 3 0 16384 7 1 8192 -1     ; Square wave
f 4 0 16384 7 1 8192 0 -1   ; Triangle wave

; Causes Csound to run indefinitely
f0 z
</CsScore>
</CsoundSynthesizer>
