# Non-verbal Cues in HRI

## Debug and Test

Build scenes: ExperimentScene_Intro, ExperimentScene_Exp.

In ExperimentScene_Exp -> (Hierarchy) EffectMesh -> (Inspector) EffectMesh -> MeshMaterial:
    - RoomBoxEffects: Will show the room and furnitures in boxes. You can use this to test whether the space information is successfully loaded.
    - SelectivePassthrough: This will make the furnitures unseen by the user, but occlude the unity objects behind them. It is used in the user study to avoid robots overlaid on the furniture.