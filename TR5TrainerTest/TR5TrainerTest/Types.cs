using System;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable FieldCanBeMadeReadOnly.Global
#pragma warning disable 169

namespace TR5TrainerTest
{
    #region Enums

    public enum zone_type
    {
        SKELLY_ZONE = 0,
        BASIC_ZONE = 1,
        CROC_ZONE = 2,
        HUMAN_ZONE = 3,
        FLYER_ZONE = 4,
    };

    public enum camera_type
    {
        CHASE_CAMERA = 0,
        FIXED_CAMERA = 1,
        LOOK_CAMERA = 2,
        COMBAT_CAMERA = 3,
        CINEMATIC_CAMERA = 4,
        HEAVY_CAMERA = 5,
    };

    public enum mood_type
    {
        BORED_MOOD = 0,
        ATTACK_MOOD = 1,
        ESCAPE_MOOD = 2,
        STALK_MOOD = 3,
    };

    public enum target_type
    {
        NO_TARGET = 0,
        PRIME_TARGET = 1,
        SECONDARY_TARGET = 2,
    };

    [Flags]
    public enum room_flags
    {
        RF_FILL_WATER = (1 << 0), // 0x0001
        RF_SKYBOX_VISIBLE = (1 << 3), // 0x0008   speeds up rendering if no rendered room has this
        RF_WIND_BLOWS_PONYTAIL = (1 << 5), // 0x0020   also some particles
        RF_UNKNOWN_6 = (1 << 6), // 0x0040   used in official levels, no apparent effects
        RF_HIDE_GLOBAL_LENS_FLARE = (1 << 7), // 0x0080   TRLE "NL"
        RF_CAUSTICS_EFFECT = (1 << 8), // 0x0100   TRLE "M"
        RF_WATER_REFLECTIVITY = (1 << 9), // 0x0200   TRLE "R"
        RF_UNKNOWN_10 = (1 << 10), // 0x0400   NGLE uses it for snow
        RF_TRLE_D = (1 << 11), // 0x0800   NGLE uses it for rain
        RF_TRLE_P = (1 << 12) // 0x1000   NGLE uses it for cold rooms
    };

    public enum item_status
    {
        ITEM_INACTIVE = 0,
        ITEM_ACTIVE = 1,
        ITEM_DEACTIVATED = 2,
        ITEM_INVISIBLE = 3
    };

    public enum material_index
    {
        MAT_MUD,
        MAT_SNOW,
        MAT_SAND,
        MAT_GRAVEL,
        MAT_ICE,
        MAT_WATER, // Unused
        MAT_STONE, // Unused
        MAT_WOOD,
        MAT_METAL,
        MAT_MARBLE,
        MAT_GRASS, // Same SFX as sand
        MAT_CONCRETE, // Same SFX as stone
        MAT_OLD_WOOD, // Same SFX as wood
        MAT_OLD_METAL, // Same SFX as metal

        NUM_MATERIALS
    };

    public enum weather_type
    {
        WEATHER_NORMAL = 0,
        WEATHER_RAIN = 1,
        WEATHER_SNOW = 2
    };

    public enum command_types
    {
        COMMAND_NULL = 0,
        COMMAND_MOVE_ORIGIN,
        COMMAND_JUMP_VELOCITY,
        COMMAND_ATTACK_READY,
        COMMAND_DEACTIVATE,
        COMMAND_SOUND_FX,
        COMMAND_EFFECT
    };

    #endregion

    #region Structs

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct VECTOR
    {
        public int vx;
        public int vy;
        public int vz;
        public int pad;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SVECTOR
    {
        public short vx;
        public short vy;
        public short vz;
        public short pad;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CVECTOR
    {
        public byte r;
        public byte g;
        public byte b;
        public byte cd;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PHD_VECTOR
    {
        public int x;
        public int y;
        public int z;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PHD_3DPOS
    {
        public int x_pos; // off 0 [64]
        public int y_pos; // off 4 [68]
        public int z_pos; // off 8 [72]
        public short x_rot; // off 12 [76]
        public short y_rot; // off 14 [78]
        public short z_rot; // off 16 [80]
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ILIGHT
    {
        public short x;
        public short y;
        public short z;
        public short pad1;
        public byte r;
        public byte g;
        public byte b;
        public byte pad;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ITEM_LIGHT
    {
        public fixed byte Light[4 * 12];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FLOOR_INFO
    {
        public ushort index; // size=0, offset=0
        public ushort fx_box_stopper;
        public byte pit_room; // size=0, offset=4
        public sbyte floor; // size=0, offset=5
        public byte sky_room; // size=0, offset=6
        public sbyte ceiling; // size=0, offset=7
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LIGHTINFO
    {
        public int x; // size=0, offset=0
        public int y; // size=0, offset=4
        public int z; // size=0, offset=8
        public byte Type; // size=0, offset=12
        public byte r; // size=0, offset=13
        public byte g; // size=0, offset=14
        public byte b; // size=0, offset=15
        public short nx; // size=0, offset=16
        public short ny; // size=0, offset=18
        public short nz; // size=0, offset=20
        public short Intensity; // size=0, offset=22
        public byte Inner; // size=0, offset=24
        public byte Outer; // size=0, offset=25
        public short FalloffScale; // size=0, offset=26
        public short Length; // size=0, offset=28
        public short Cutoff; // size=0, offset=30
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MESH_INFO
    {
        public int x; // size=0, offset=0
        public int y; // size=0, offset=4
        public int z; // size=0, offset=8
        public short y_rot; // size=0, offset=12
        public short shade; // size=0, offset=14
        public short Flags; // size=0, offset=16
        public short static_number; // size=0, offset=18
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FX_INFO
    {
        public PHD_3DPOS pos; // size=20, offset=0
        public short room_number; // size=0, offset=20
        public short object_number; // size=0, offset=22
        public short next_fx; // size=0, offset=24
        public short next_active; // size=0, offset=26
        public short speed; // size=0, offset=28
        public short fallspeed; // size=0, offset=30
        public short frame_number; // size=0, offset=32
        public short counter; // size=0, offset=34
        public short shade; // size=0, offset=36
        public short flag1; // size=0, offset=38
        public short flag2; // size=0, offset=40
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct lara_arm
    {
        public short* frame_base; // size=0, offset=0
        public short frame_number; // size=0, offset=4
        public short anim_number; // size=0, offset=6
        public short _lock; // size=0, offset=8
        public short y_rot; // size=0, offset=10
        public short x_rot; // size=0, offset=12
        public short z_rot; // size=0, offset=14
        public short flash_gun; // size=0, offset=16
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct box_node
    {
        public short exit_box; // size=0, offset=0
        public ushort search_number; // size=0, offset=2
        public short next_expansion; // size=0, offset=4
        public short box_number; // size=0, offset=6
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct lot_info
    {
        public box_node* node; // size=8, offset=0
        public short head; // size=0, offset=4
        public short tail; // size=0, offset=6
        public ushort search_number; // size=0, offset=8
        public ushort block_mask; // size=0, offset=10
        public short step; // size=0, offset=12
        public short drop; // size=0, offset=14
        public short zone_count; // size=0, offset=16
        public short target_box; // size=0, offset=18
        public short required_box; // size=0, offset=20
        public short fly; // size=0, offset=22
        public ushort bitfield;
        public PHD_VECTOR target; // size=12, offset=28
        public zone_type zone; // size=4, offset=40
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ROPE_STRUCT
    {
        public fixed byte Segment[24 * 12]; // size=288, offset=0
        public fixed byte Velocity[24 * 12]; // size=288, offset=288
        public fixed byte NormalisedSegment[24 * 12]; // size=288, offset=576
        public fixed byte MeshSegment[24 * 12]; // size=288, offset=864
        public PHD_VECTOR Position; // size=12, offset=1152
        public int SegmentLength; // size=0, offset=1164
        public short Active; // size=0, offset=1168
        public short Coiled; // size=0, offset=1170
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct lara_info
    {
        public short item_number; // size=0, offset=0
        public short gun_status; // size=0, offset=2
        public short gun_type; // size=0, offset=4
        public short request_gun_type; // size=0, offset=6
        public short last_gun_type; // size=0, offset=8
        public short calc_fallspeed; // size=0, offset=10
        public short water_status; // size=0, offset=12
        public short climb_status; // size=0, offset=14
        public short pose_count; // size=0, offset=16
        public short hit_frame; // size=0, offset=18
        public short hit_direction; // size=0, offset=20
        public short air; // size=0, offset=22
        public short dive_count; // size=0, offset=24
        public short death_count; // size=0, offset=26
        public short current_active; // size=0, offset=28
        public short current_xvel; // size=0, offset=30
        public short current_yvel; // size=0, offset=32
        public short current_zvel; // size=0, offset=34
        public short spaz_effect_count; // size=0, offset=36
        public short flare_age; // size=0, offset=38
        public short BurnCount; // size=0, offset=40
        public short weapon_item; // size=0, offset=42
        public short back_gun; // size=0, offset=44
        public short flare_frame; // size=0, offset=46
        public short poisoned; // size=0, offset=48
        public short dpoisoned; // size=0, offset=50
        public byte Anxiety; // size=0, offset=52
        public fixed byte wet[15]; // size=15, offset=53
        public ushort bitfield1;
        public ushort bitfield2;
        public int water_surface_dist; // size=0, offset=72
        public PHD_VECTOR last_pos; // size=12, offset=76

        public FX_INFO* spaz_effect; // size=44, offset=88
        public int mesh_effects; // size=0, offset=92

        public fixed int mesh_ptrs[15]; // size=60, offset=96
        public ITEM_INFO* target; // size=144, offset=156
        public fixed short target_angles[2]; // size=4, offset=160
        public short turn_rate; // size=0, offset=164
        public short move_angle; // size=0, offset=166
        public short head_y_rot; // size=0, offset=168
        public short head_x_rot; // size=0, offset=170
        public short head_z_rot; // size=0, offset=172
        public short torso_y_rot; // size=0, offset=174
        public short torso_x_rot; // size=0, offset=176
        public short torso_z_rot; // size=0, offset=178
        public lara_arm left_arm; // size=20, offset=180
        public lara_arm right_arm; // size=20, offset=200
        public ushort holster; // size=0, offset=220
        public creature_info* creature; // size=228, offset=224
        public int CornerX; // size=0, offset=228
        public int CornerZ; // size=0, offset=232
        public sbyte RopeSegment; // size=0, offset=236
        public sbyte RopeDirection; // size=0, offset=237
        public short RopeArcFront; // size=0, offset=238
        public short RopeArcBack; // size=0, offset=240
        public short RopeLastX; // size=0, offset=242
        public short RopeMaxXForward; // size=0, offset=244
        public short RopeMaxXBackward; // size=0, offset=246
        public int RopeDFrame; // size=0, offset=248
        public int RopeFrame; // size=0, offset=252
        public ushort RopeFrameRate; // size=0, offset=256
        public ushort RopeY; // size=0, offset=258
        public int RopePtr; // size=0, offset=260
        public void* GeneralPtr; // size=0, offset=264
        public int RopeOffset; // size=0, offset=268
        public uint RopeDownVel; // size=0, offset=272
        public sbyte RopeFlag; // size=0, offset=276
        public sbyte MoveCount; // size=0, offset=277
        public int RopeCount; // size=0, offset=280
        public sbyte skelebob; // size=0, offset=284
        public sbyte pistols_type_carried; // size=0, offset=285
        public sbyte uzis_type_carried; // size=0, offset=286
        public sbyte shotgun_type_carried; // size=0, offset=287
        public sbyte crossbow_type_carried; // size=0, offset=288
        public sbyte hk_type_carried; // size=0, offset=289
        public sbyte sixshooter_type_carried; // size=0, offset=290
        public sbyte lasersight; // size=0, offset=291
        public sbyte silencer; // size=0, offset=292
        public sbyte binoculars; // size=0, offset=293
        public sbyte crowbar; // size=0, offset=294
        public sbyte examine1; // size=0, offset=295
        public sbyte examine2; // size=0, offset=296
        public sbyte examine3; // size=0, offset=297
        public sbyte wetcloth; // size=0, offset=298
        public sbyte bottle; // size=0, offset=299
        public fixed sbyte puzzleitems[12]; // size=12, offset=300
        public ushort puzzleitemscombo; // size=0, offset=312
        public ushort keyitems; // size=0, offset=314
        public ushort keyitemscombo; // size=0, offset=316
        public ushort pickupitems; // size=0, offset=318
        public ushort pickupitemscombo; // size=0, offset=320
        public short num_small_medipack; // size=0, offset=322
        public short num_large_medipack; // size=0, offset=324
        public short num_flares; // size=0, offset=326
        public short num_pistols_ammo; // size=0, offset=328
        public short num_uzi_ammo; // size=0, offset=330
        public short num_revolver_ammo; // size=0, offset=332
        public short num_shotgun_ammo1; // size=0, offset=334
        public short num_shotgun_ammo2; // size=0, offset=336
        public short num_hk_ammo1; // size=0, offset=338
        public short num_crossbow_ammo1; // size=0, offset=340
        public short num_crossbow_ammo2; // size=0, offset=342
        public sbyte location; // size=0, offset=344
        public sbyte highest_location; // size=0, offset=345
        public sbyte locationPad; // size=0, offset=346
        public byte TightRopeOnCount; // size=0, offset=347
        public byte TightRopeOff; // size=0, offset=348
        public byte TightRopeFall; // size=0, offset=349
        public byte ChaffTimer; // size=0, offset=350
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct STATS
    {
        public uint Timer; // size=0, offset=0
        public uint Distance; // size=0, offset=4
        public uint AmmoUsed; // size=0, offset=8
        public uint AmmoHits; // size=0, offset=12
        public ushort Kills; // size=0, offset=16
        public byte Secrets; // size=0, offset=18
        public byte HealthUsed; // size=0, offset=19
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct savegame_info
    {
        public short Checksum; // size=0, offset=0
        public ushort VolumeCD; // size=0, offset=2
        public ushort VolumeFX; // size=0, offset=4
        public short ScreenX; // size=0, offset=6
        public short ScreenY; // size=0, offset=8
        public byte ControlOption; // size=0, offset=10
        public byte VibrateOn; // size=0, offset=11
        public byte AutoTarget; // size=0, offset=12
        public lara_info Lara; // size=352, offset=16
        public STATS Level; // size=20, offset=368
        public STATS Game; // size=20, offset=388
        public short WeaponObject; // size=0, offset=408
        public short WeaponAnim; // size=0, offset=410
        public short WeaponFrame; // size=0, offset=412
        public short WeaponCurrent; // size=0, offset=414
        public short WeaponGoal; // size=0, offset=416
        public uint CutSceneTriggered1; // size=0, offset=420
        public uint CutSceneTriggered2; // size=0, offset=424
        public sbyte GameComplete; // size=0, offset=428
        public byte CurrentLevel; // size=0, offset=429
        public fixed byte CampaignSecrets[4]; // size=4, offset=430
        public byte TLCount; // size=0, offset=434
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MATRIX3D
    {
        public short m00; // size=0, offset=0
        public short m01; // size=0, offset=2
        public short m02; // size=0, offset=4
        public short m10; // size=0, offset=6
        public short m11; // size=0, offset=8
        public short m12; // size=0, offset=10
        public short m20; // size=0, offset=12
        public short m21; // size=0, offset=14
        public short m22; // size=0, offset=16
        public short pad; // size=0, offset=18
        public int tx; // size=0, offset=20
        public int ty; // size=0, offset=24
        public int tz; // size=0, offset=28
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GAMEFLOW
    {
        public uint bitfield;
        public uint InputTimeout; // size=0, offset=4
        public byte SecurityTag; // size=0, offset=8
        public byte nLevels; // size=0, offset=9
        public byte nFileNames; // size=0, offset=10
        public byte Pad; // size=0, offset=11
        public ushort FileNameLen; // size=0, offset=12
        public ushort ScriptLen; // size=0, offset=14
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct STRINGHEADER
    {
        public ushort nStrings; // size=0, offset=0
        public ushort nPSXStrings; // size=0, offset=2
        public ushort nPCStrings; // size=0, offset=4
        public ushort StringWadLen; // size=0, offset=6
        public ushort PSXStringWadLen; // size=0, offset=8
        public ushort PCStringWadLen; // size=0, offset=10
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GAME_VECTOR
    {
        public int x; // size=0, offset=0
        public int y; // size=0, offset=4
        public int z; // size=0, offset=8
        public short room_number; // size=0, offset=12
        public short box_number; // size=0, offset=14
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct OBJECT_VECTOR
    {
        public int x; // size=0, offset=0
        public int y; // size=0, offset=4
        public int z; // size=0, offset=8
        public short data; // size=0, offset=12
        public short flags; // size=0, offset=14
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SPHERE
    {
        public int x; // size=0, offset=0
        public int y; // size=0, offset=4
        public int z; // size=0, offset=8
        public int r; // size=0, offset=12
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct PENDULUM
    {
        public PHD_VECTOR Position; // size=12, offset=0
        public PHD_VECTOR Velocity; // size=12, offset=12
        public int node; // size=0, offset=24
        public ROPE_STRUCT* Rope; // size=1172, offset=28
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RAT_STRUCT
    {
        public PHD_3DPOS pos; // size=20, offset=0
        public short room_number; // size=0, offset=20
        public short speed; // size=0, offset=22
        public short fallspeed; // size=0, offset=24
        public byte On; // size=0, offset=26
        public byte flags; // size=0, offset=27
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BAT_STRUCT
    {
        public PHD_3DPOS pos; // size=20, offset=0
        public short room_number; // size=0, offset=20
        public short speed; // size=0, offset=22
        public short Counter; // size=0, offset=24
        public short LaraTarget; // size=0, offset=26
        public sbyte XTarget; // size=0, offset=28
        public sbyte ZTarget; // size=0, offset=29
        public byte On; // size=0, offset=30
        public byte flags; // size=0, offset=31
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SPIDER_STRUCT
    {
        public PHD_3DPOS pos; // size=20, offset=0
        public short room_number; // size=0, offset=20
        public short speed; // size=0, offset=22
        public short fallspeed; // size=0, offset=24
        public byte On; // size=0, offset=26
        public byte flags; // size=0, offset=27
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TWOGUN_INFO
    {
        public PHD_3DPOS pos; // size=20, offset=0
        public short life; // size=0, offset=20
        public short coil; // size=0, offset=22
        public short spin; // size=0, offset=24
        public short spinadd; // size=0, offset=26
        public short length; // size=0, offset=28
        public short dlength; // size=0, offset=30
        public short size; // size=0, offset=32
        public sbyte r; // size=0, offset=34
        public sbyte g; // size=0, offset=35
        public sbyte b; // size=0, offset=36
        public sbyte fadein; // size=0, offset=37
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct sbyteDEF
    {
        public byte u; // size=0, offset=0
        public byte v; // size=0, offset=1
        public byte w; // size=0, offset=2
        public byte h; // size=0, offset=3
        public sbyte YOffset; // size=0, offset=4
        public byte TopShade; // size=0, offset=5
        public byte BottomShade; // size=0, offset=6
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct COLL_INFO
    {
        public int mid_floor; // size=0, offset=0
        public int mid_ceiling; // size=0, offset=4
        public int mid_type; // size=0, offset=8
        public int front_floor; // size=0, offset=12
        public int front_ceiling; // size=0, offset=16
        public int front_type; // size=0, offset=20
        public int left_floor; // size=0, offset=24
        public int left_ceiling; // size=0, offset=28
        public int left_type; // size=0, offset=32
        public int right_floor; // size=0, offset=36
        public int right_ceiling; // size=0, offset=40
        public int right_type; // size=0, offset=44
        public int left_floor2; // size=0, offset=48
        public int left_ceiling2; // size=0, offset=52
        public int left_type2; // size=0, offset=56
        public int right_floor2; // size=0, offset=60
        public int right_ceiling2; // size=0, offset=64
        public int right_type2; // size=0, offset=68
        public int radius; // size=0, offset=72
        public int bad_pos; // size=0, offset=76
        public int bad_neg; // size=0, offset=80
        public int bad_ceiling; // size=0, offset=84
        public PHD_VECTOR shift; // size=12, offset=88
        public PHD_VECTOR old; // size=12, offset=100
        public short old_anim_state; // size=0, offset=112
        public short old_anim_number; // size=0, offset=114
        public short old_frame_number; // size=0, offset=116
        public short facing; // size=0, offset=118
        public short quadrant; // size=0, offset=120
        public short coll_type; // size=0, offset=122 USE public enum CT_*
        public short* trigger; // size=0, offset=124
        public sbyte tilt_x; // size=0, offset=128
        public sbyte tilt_z; // size=0, offset=129
        public sbyte hit_by_baddie; // size=0, offset=130
        public sbyte hit_static; // size=0, offset=131
        public ushort bitfield;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ANIM_STRUCT
    {
        public short* frame_ptr; // size=0, offset=0
        public short interpolation; // size=0, offset=4
        public short current_anim_state; // size=0, offset=6
        public int velocity; // size=0, offset=8
        public int acceleration; // size=0, offset=12
        public int Xvelocity; // size=0, offset=16
        public int Xacceleration; // size=0, offset=20
        public short frame_base; // size=0, offset=24
        public short frame_end; // size=0, offset=26
        public short jump_anim_num; // size=0, offset=28
        public short jump_frame_num; // size=0, offset=30
        public short number_changes; // size=0, offset=32
        public short change_index; // size=0, offset=34
        public short number_commands; // size=0, offset=36
        public short command_index; // size=0, offset=38
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SPARKS
    {
        public int x; // size=0, offset=0
        public int y; // size=0, offset=4
        public int z; // size=0, offset=8
        public short Xvel; // size=0, offset=12
        public short Yvel; // size=0, offset=14
        public short Zvel; // size=0, offset=16
        public short Gravity; // size=0, offset=18
        public short RotAng; // size=0, offset=20
        public short Flags; // size=0, offset=22
        public byte sSize; // size=0, offset=24
        public byte dSize; // size=0, offset=25
        public byte Size; // size=0, offset=26
        public byte Friction; // size=0, offset=27
        public byte Scalar; // size=0, offset=28
        public byte Def; // size=0, offset=29
        public sbyte RotAdd; // size=0, offset=30
        public sbyte MaxYvel; // size=0, offset=31
        public byte On; // size=0, offset=32
        public byte sR; // size=0, offset=33
        public byte sG; // size=0, offset=34
        public byte sB; // size=0, offset=35
        public byte dR; // size=0, offset=36
        public byte dG; // size=0, offset=37
        public byte dB; // size=0, offset=38
        public byte R; // size=0, offset=39
        public byte G; // size=0, offset=40
        public byte B; // size=0, offset=41
        public byte ColFadeSpeed; // size=0, offset=42
        public byte FadeToBlack; // size=0, offset=43
        public byte sLife; // size=0, offset=44
        public byte Life; // size=0, offset=45
        public byte TransType; // size=0, offset=46
        public byte extras; // size=0, offset=47
        public sbyte Dynamic; // size=0, offset=48
        public byte FxObj; // size=0, offset=49
        public byte RoomNumber; // size=0, offset=50
        public byte NodeNumber; // size=0, offset=51
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ACTORME
    {
        public int offset; // size=0, offset=0
        public short objslot; // size=0, offset=4
        public short nodes; // size=0, offset=6
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RTDECODE
    {
        public uint length; // size=0, offset=0
        public uint off; // size=0, offset=4
        public ushort counter; // size=0, offset=8
        public ushort data; // size=0, offset=10
        public byte decodetype; // size=0, offset=12
        public byte packmethod; // size=0, offset=13
        public ushort padfuck; // size=0, offset=14
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct PACKNODE
    {
        public short xrot_run; // size=0, offset=0
        public short yrot_run; // size=0, offset=2
        public short zrot_run; // size=0, offset=4
        public short xkey; // size=0, offset=6
        public short ykey; // size=0, offset=8
        public short zkey; // size=0, offset=10
        public RTDECODE decode_x; // size=16, offset=12
        public RTDECODE decode_y; // size=16, offset=28
        public RTDECODE decode_z; // size=16, offset=44
        public uint xlength; // size=0, offset=60
        public uint ylength; // size=0, offset=64
        public uint zlength; // size=0, offset=68
        public sbyte* xpacked; // size=0, offset=72
        public sbyte* ypacked; // size=0, offset=76
        public sbyte* zpacked; // size=0, offset=80
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NODELOADHEADER
    {
        public short xkey; // size=0, offset=0
        public short ykey; // size=0, offset=2
        public short zkey; // size=0, offset=4
        public short packmethod; // size=0, offset=6
        public short xlength; // size=0, offset=8
        public short ylength; // size=0, offset=10
        public short zlength; // size=0, offset=12
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct RESIDENT_THING
    {
        public sbyte* packed_data; // size=0, offset=0
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct NEW_CUTSCENE
    {
        public short numactors; // size=0, offset=0
        public short numframes; // size=0, offset=2
        public int orgx; // size=0, offset=4
        public int orgy; // size=0, offset=8
        public int orgz; // size=0, offset=12
        public int audio_track; // size=0, offset=16
        public int camera_offset; // size=0, offset=20
        public fixed byte actor_data[10 * 8]; // size=80, offset=24
    };

    public delegate void cutseq_func();

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CUTSEQ_ROUTINES
    {
        public cutseq_func init_func; // size=0, offset=0
        public cutseq_func control_func; // size=0, offset=4
        public cutseq_func end_func; // size=0, offset=8
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct DOORPOS_DATA
    {
        public FLOOR_INFO* floor; // size=8, offset=0

        public FLOOR_INFO data; // size=8, offset=4
        public short block; // size=0, offset=12
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct DOOR_DATA
    {
        public DOORPOS_DATA d1; // size=16, offset=0
        public DOORPOS_DATA d1flip; // size=16, offset=16
        public DOORPOS_DATA d2; // size=16, offset=32
        public DOORPOS_DATA d2flip; // size=16, offset=48
        public short Opened; // size=0, offset=64
        public short* dptr1; // size=0, offset=68
        public short* dptr2; // size=0, offset=72
        public short* dptr3; // size=0, offset=76
        public short* dptr4; // size=0, offset=80
        public sbyte dn1; // size=0, offset=84
        public sbyte dn2; // size=0, offset=85
        public sbyte dn3; // size=0, offset=86
        public sbyte dn4; // size=0, offset=87
        public ITEM_INFO* item; // size=144, offset=88
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CHANGE_STRUCT
    {
        public short goal_anim_state; // size=0, offset=0
        public short number_ranges; // size=0, offset=2
        public short range_index; // size=0, offset=4
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RANGE_STRUCT
    {
        public short start_frame; // size=0, offset=0
        public short end_frame; // size=0, offset=2
        public short link_anim_num; // size=0, offset=4
        public short link_frame_num; // size=0, offset=6
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct door_vbuf
    {
        public int xv; // size=0, offset=0
        public int yv; // size=0, offset=4
        public int zv; // size=0, offset=8
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BITE_INFO
    {
        public int x; // size=0, offset=0
        public int y; // size=0, offset=4
        public int z; // size=0, offset=8
        public int mesh_num; // size=0, offset=12
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct box_info
    {
        public byte left; // size=0, offset=0
        public byte right; // size=0, offset=1
        public byte top; // size=0, offset=2
        public byte bottom; // size=0, offset=3
        public short height; // size=0, offset=4
        public short overlap_index; // size=0, offset=6
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AI_info
    {
        public short zone_number; // size=0, offset=0
        public short enemy_zone; // size=0, offset=2
        public int distance; // size=0, offset=4
        public int ahead; // size=0, offset=8
        public int bite; // size=0, offset=12
        public short angle; // size=0, offset=16
        public short x_angle; // size=0, offset=18
        public short enemy_facing; // size=0, offset=20
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AIOBJECT
    {
        public short object_number; // size=0, offset=0
        public short room_number; // size=0, offset=2
        public int x; // size=0, offset=4
        public int y; // size=0, offset=8
        public int z; // size=0, offset=12
        public short trigger_flags; // size=0, offset=16
        public short flags; // size=0, offset=18
        public short y_rot; // size=0, offset=20
        public short box_number; // size=0, offset=22
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct CAMERA_INFO
    {
        public GAME_VECTOR pos; // size=16, offset=0
        public GAME_VECTOR target; // size=16, offset=16
        public camera_type type; // size=4, offset=32
        public camera_type old_type; // size=4, offset=36
        public int shift; // size=0, offset=40
        public int flags; // size=0, offset=44
        public int fixed_camera; // size=0, offset=48
        public int number_frames; // size=0, offset=52
        public int bounce; // size=0, offset=56
        public int underwater; // size=0, offset=60
        public int target_distance; // size=0, offset=64
        public short target_angle; // size=0, offset=68
        public short target_elevation; // size=0, offset=70
        public short actual_elevation; // size=0, offset=72
        public short actual_angle; // size=0, offset=74
        public short lara_node; // size=0, offset=76
        public short box; // size=0, offset=78
        public short number; // size=0, offset=80
        public short last; // size=0, offset=82
        public short timer; // size=0, offset=84
        public short speed; // size=0, offset=86
        public short targetspeed; // size=0, offset=88
        public ITEM_INFO* item; // size=144, offset=92
        public ITEM_INFO* last_item; // size=144, offset=96
        public OBJECT_VECTOR* _fixed; // size=16, offset=100
        public int mike_at_lara; // size=0, offset=104
        public PHD_VECTOR mike_pos; // size=12, offset=108
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SHATTER_ITEM
    {
        public SPHERE Sphere; // size=16, offset=0
        public ITEM_LIGHT* il; // size=48, offset=16
        public short* meshp; // size=0, offset=20
        public int Bit; // size=0, offset=24
        public short YRot; // size=0, offset=28
        public short Flags; // size=0, offset=30
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct OLD_CAMERA
    {
        public short current_anim_state; // size=0, offset=0
        public short goal_anim_state; // size=0, offset=2
        public int target_distance; // size=0, offset=4
        public short target_angle; // size=0, offset=8
        public short target_elevation; // size=0, offset=10
        public short actual_elevation; // size=0, offset=12
        public PHD_3DPOS pos; // size=20, offset=16
        public PHD_3DPOS pos2; // size=20, offset=36
        public PHD_VECTOR t; // size=12, offset=56
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct DEBRIS_STRUCT
    {
        public void* TextInfo; // size=0, offset=0
        public int x; // size=0, offset=4
        public int y; // size=0, offset=8
        public int z; // size=0, offset=12
        public fixed short XYZOffsets1[3]; // size=6, offset=16
        public short Dir; // size=0, offset=22
        public fixed short XYZOffsets2[3]; // size=6, offset=24
        public short Speed; // size=0, offset=30
        public fixed short XYZOffsets3[3]; // size=6, offset=32
        public short Yvel; // size=0, offset=38
        public short Gravity; // size=0, offset=40
        public short RoomNumber; // size=0, offset=42
        public byte On; // size=0, offset=44
        public byte XRot; // size=0, offset=45
        public byte YRot; // size=0, offset=46
        public byte r; // size=0, offset=47
        public byte g; // size=0, offset=48
        public byte b; // size=0, offset=49
        public fixed byte Pad[2]; // size=2, offset=50
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SPOTCAM
    {
        public int x; // size=0, offset=0
        public int y; // size=0, offset=4
        public int z; // size=0, offset=8
        public int tx; // size=0, offset=12
        public int ty; // size=0, offset=16
        public int tz; // size=0, offset=20
        public byte sequence; // size=0, offset=24
        public byte camera; // size=0, offset=25
        public short fov; // size=0, offset=26
        public short roll; // size=0, offset=28
        public short timer; // size=0, offset=30
        public short speed; // size=0, offset=32
        public short flags; // size=0, offset=34
        public short room_number; // size=0, offset=36
        public short pad; // size=0, offset=38
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct QUAKE_CAM
    {
        public GAME_VECTOR spos; // size=16, offset=0
        public GAME_VECTOR epos; // size=16, offset=16
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct DYNAMIC
    {
        public int x; // size=0, offset=0
        public int y; // size=0, offset=4
        public int z; // size=0, offset=8
        public byte on; // size=0, offset=12
        public byte r; // size=0, offset=13
        public byte g; // size=0, offset=14
        public byte b; // size=0, offset=15
        public ushort falloff; // size=0, offset=16
        public byte used; // size=0, offset=18
        public fixed byte pad1[1]; // size=1, offset=19
        public int FalloffScale; // size=0, offset=20
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SP_DYNAMIC
    {
        public byte On; // size=0, offset=0
        public byte Falloff; // size=0, offset=1
        public byte R; // size=0, offset=2
        public byte G; // size=0, offset=3
        public byte B; // size=0, offset=4
        public byte Flags; // size=0, offset=5
        public fixed byte Pad[2]; // size=2, offset=6
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SPLASH_STRUCT
    {
        public int x; // size=0, offset=0
        public int y; // size=0, offset=4
        public int z; // size=0, offset=8
        public short InnerRad; // size=0, offset=12
        public short InnerSize; // size=0, offset=14
        public short InnerRadVel; // size=0, offset=16
        public short InnerYVel; // size=0, offset=18
        public short InnerY; // size=0, offset=20
        public short MiddleRad; // size=0, offset=22
        public short MiddleSize; // size=0, offset=24
        public short MiddleRadVel; // size=0, offset=26
        public short MiddleYVel; // size=0, offset=28
        public short MiddleY; // size=0, offset=30
        public short OuterRad; // size=0, offset=32
        public short OuterSize; // size=0, offset=34
        public short OuterRadVel; // size=0, offset=36
        public sbyte flags; // size=0, offset=38
        public byte life; // size=0, offset=39
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RIPPLE_STRUCT
    {
        public int x; // size=0, offset=0
        public int y; // size=0, offset=4
        public int z; // size=0, offset=8
        public sbyte flags; // size=0, offset=12
        public byte life; // size=0, offset=13
        public byte size; // size=0, offset=14
        public byte init; // size=0, offset=15
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SPLASH_SETUP
    {
        public int x; // size=0, offset=0
        public int y; // size=0, offset=4
        public int z; // size=0, offset=8
        public short InnerRad; // size=0, offset=12
        public short InnerSize; // size=0, offset=14
        public short InnerRadVel; // size=0, offset=16
        public short InnerYVel; // size=0, offset=18
        public short pad1; // size=0, offset=20
        public short MiddleRad; // size=0, offset=22
        public short MiddleSize; // size=0, offset=24
        public short MiddleRadVel; // size=0, offset=26
        public short MiddleYVel; // size=0, offset=28
        public short pad2; // size=0, offset=30
        public short OuterRad; // size=0, offset=32
        public short OuterSize; // size=0, offset=34
        public short OuterRadVel; // size=0, offset=36
        public short pad3; // size=0, offset=38
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FIRE_LIST
    {
        public int x; // size=0, offset=0
        public int y; // size=0, offset=4
        public int z; // size=0, offset=8
        public sbyte on; // size=0, offset=12
        public sbyte size; // size=0, offset=13
        public short room_number; // size=0, offset=14
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FIRE_SPARKS
    {
        public short x; // size=0, offset=0
        public short y; // size=0, offset=2
        public short z; // size=0, offset=4
        public short Xvel; // size=0, offset=6
        public short Yvel; // size=0, offset=8
        public short Zvel; // size=0, offset=10
        public short Gravity; // size=0, offset=12
        public short RotAng; // size=0, offset=14
        public short Flags; // size=0, offset=16
        public byte sSize; // size=0, offset=18
        public byte dSize; // size=0, offset=19
        public byte Size; // size=0, offset=20
        public byte Friction; // size=0, offset=21
        public byte Scalar; // size=0, offset=22
        public byte Def; // size=0, offset=23
        public sbyte RotAdd; // size=0, offset=24
        public sbyte MaxYvel; // size=0, offset=25
        public byte On; // size=0, offset=26
        public byte sR; // size=0, offset=27
        public byte sG; // size=0, offset=28
        public byte sB; // size=0, offset=29
        public byte dR; // size=0, offset=30
        public byte dG; // size=0, offset=31
        public byte dB; // size=0, offset=32
        public byte R; // size=0, offset=33
        public byte G; // size=0, offset=34
        public byte B; // size=0, offset=35
        public byte ColFadeSpeed; // size=0, offset=36
        public byte FadeToBlack; // size=0, offset=37
        public byte sLife; // size=0, offset=38
        public byte Life; // size=0, offset=39
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SMOKE_SPARKS
    {
        public int x; // size=0, offset=0
        public int y; // size=0, offset=4
        public int z; // size=0, offset=8
        public short Xvel; // size=0, offset=12
        public short Yvel; // size=0, offset=14
        public short Zvel; // size=0, offset=16
        public short Gravity; // size=0, offset=18
        public short RotAng; // size=0, offset=20
        public short Flags; // size=0, offset=22
        public byte sSize; // size=0, offset=24
        public byte dSize; // size=0, offset=25
        public byte Size; // size=0, offset=26
        public byte Friction; // size=0, offset=27
        public byte Scalar; // size=0, offset=28
        public byte Def; // size=0, offset=29
        public sbyte RotAdd; // size=0, offset=30
        public sbyte MaxYvel; // size=0, offset=31
        public byte On; // size=0, offset=32
        public byte sShade; // size=0, offset=33
        public byte dShade; // size=0, offset=34
        public byte Shade; // size=0, offset=35
        public byte ColFadeSpeed; // size=0, offset=36
        public byte FadeToBlack; // size=0, offset=37
        public sbyte sLife; // size=0, offset=38
        public sbyte Life; // size=0, offset=39
        public byte TransType; // size=0, offset=40
        public byte FxObj; // size=0, offset=41
        public byte NodeNumber; // size=0, offset=42
        public byte mirror; // size=0, offset=43
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BLOOD_STRUCT
    {
        public int x; // size=0, offset=0
        public int y; // size=0, offset=4
        public int z; // size=0, offset=8
        public short Xvel; // size=0, offset=12
        public short Yvel; // size=0, offset=14
        public short Zvel; // size=0, offset=16
        public short Gravity; // size=0, offset=18
        public short RotAng; // size=0, offset=20
        public byte sSize; // size=0, offset=22
        public byte dSize; // size=0, offset=23
        public byte Size; // size=0, offset=24
        public byte Friction; // size=0, offset=25
        public sbyte RotAdd; // size=0, offset=26
        public byte On; // size=0, offset=27
        public byte sShade; // size=0, offset=28
        public byte dShade; // size=0, offset=29
        public byte Shade; // size=0, offset=30
        public byte ColFadeSpeed; // size=0, offset=31
        public byte FadeToBlack; // size=0, offset=32
        public sbyte sLife; // size=0, offset=33
        public sbyte Life; // size=0, offset=34
        public sbyte Pad; // size=0, offset=35
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GUNSHELL_STRUCT
    {
        public PHD_3DPOS pos; // size=20, offset=0
        public short fallspeed; // size=0, offset=20
        public short room_number; // size=0, offset=22
        public short speed; // size=0, offset=24
        public short counter; // size=0, offset=26
        public short DirXrot; // size=0, offset=28
        public short object_number; // size=0, offset=30
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BUBBLE_STRUCT
    {
        public PHD_VECTOR pos; // size=12, offset=0
        public short room_number; // size=0, offset=12
        public short speed; // size=0, offset=14
        public short size; // size=0, offset=16
        public short dsize; // size=0, offset=18
        public byte shade; // size=0, offset=20
        public byte vel; // size=0, offset=21
        public byte y_rot; // size=0, offset=22
        public sbyte Flags; // size=0, offset=23
        public short Xvel; // size=0, offset=24
        public short Yvel; // size=0, offset=26
        public short Zvel; // size=0, offset=28
        public short Pad; // size=0, offset=30
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GUNFLASH_STRUCT
    {
        public MATRIX3D matrix; // size=32, offset=0
        public short on; // size=0, offset=32
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NODEOFFSET_INFO
    {
        public short x; // size=0, offset=0
        public short y; // size=0, offset=2
        public short z; // size=0, offset=4
        public sbyte mesh_num; // size=0, offset=6
        public byte GotIt; // size=0, offset=7
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SHOCKWAVE_STRUCT
    {
        public int x; // size=0, offset=0
        public int y; // size=0, offset=4
        public int z; // size=0, offset=8
        public short InnerRad; // size=0, offset=12
        public short OuterRad; // size=0, offset=14
        public short XRot; // size=0, offset=16
        public short Flags; // size=0, offset=18
        public byte r; // size=0, offset=20
        public byte g; // size=0, offset=21
        public byte b; // size=0, offset=22
        public byte life; // size=0, offset=23
        public short Speed; // size=0, offset=24
        public short Temp; // size=0, offset=26
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct HAIR_STRUCT
    {
        public PHD_3DPOS pos; // size=20, offset=0
        public PHD_VECTOR vel; // size=12, offset=20
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DISPLAYPU
    {
        public short life; // size=0, offset=0
        public short object_number; // size=0, offset=2
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct INVOBJ
    {
        public short object_number; // size=0, offset=0
        public short yoff; // size=0, offset=2
        public short scale1; // size=0, offset=4
        public short yrot; // size=0, offset=6
        public short xrot; // size=0, offset=8
        public short zrot; // size=0, offset=10
        public short flags; // size=0, offset=12
        public short objname; // size=0, offset=14
        public uint meshbits; // size=0, offset=16
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct OBJLIST
    {
        public short invitem; // size=0, offset=0
        public ushort yrot; // size=0, offset=2
        public ushort bright; // size=0, offset=4
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct RINGME
    {
        public fixed byte current_object_list[100 * 6]; // size=600, offset=0
        public int ringactive; // size=0, offset=600
        public int objlistmovement; // size=0, offset=604
        public int curobjinlist; // size=0, offset=608
        public int numobjectsinlist; // size=0, offset=612
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AMMOLIST
    {
        public short invitem; // size=0, offset=0
        public short amount; // size=0, offset=2
        public ushort yrot; // size=0, offset=4
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MENUTHANG
    {
        public int type; // size=0, offset=0
        public sbyte* text; // size=0, offset=4
    };

    public delegate void combine_func(int flag);

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct COMBINELIST
    {
        public combine_func combine_routine; // size=0, offset=0
        public short item1; // size=0, offset=4
        public short item2; // size=0, offset=6
        public short combined_item; // size=0, offset=8
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct WEAPON_INFO
    {
        public fixed short lock_angles[4]; // size=8, offset=0
        public fixed short left_angles[4]; // size=8, offset=8
        public fixed short right_angles[4]; // size=8, offset=16
        public short aim_speed; // size=0, offset=24
        public short shot_accuracy; // size=0, offset=26
        public short gun_height; // size=0, offset=28
        public short target_dist; // size=0, offset=30
        public sbyte damage; // size=0, offset=32
        public sbyte recoil_frame; // size=0, offset=33
        public sbyte flash_time; // size=0, offset=34
        public sbyte draw_frame; // size=0, offset=35
        public short sample_num; // size=0, offset=36
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PISTOL_DEF
    {
        public short ObjectNum; // size=0, offset=0
        public sbyte Draw1Anim2; // size=0, offset=2
        public sbyte Draw1Anim; // size=0, offset=3
        public sbyte Draw2Anim; // size=0, offset=4
        public sbyte RecoilAnim; // size=0, offset=5
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SUBSUIT_INFO
    {
        public short XRot; // size=0, offset=0
        public short dXRot; // size=0, offset=2
        public short XRotVel; // size=0, offset=4
        public fixed short Vel[2]; // size=4, offset=6
        public short YVel; // size=0, offset=10
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SAMPLE_INFO
    {
        public short number; // size=0, offset=0
        public byte volume; // size=0, offset=2
        public sbyte radius; // size=0, offset=3
        public sbyte randomness; // size=0, offset=4
        public sbyte pitch; // size=0, offset=5
        public short flags; // size=0, offset=6
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SoundSlot
    {
        public int OrigVolume; // size=0, offset=0
        public int nVolume; // size=0, offset=4
        public int nPan; // size=0, offset=8
        public int nPitch; // size=0, offset=12
        public int nSampleInfo; // size=0, offset=16
        public uint distance; // size=0, offset=20
        public PHD_VECTOR pos; // size=12, offset=24
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FOOTPRINT
    {
        public int x; // size=0, offset=0
        public int y; // size=0, offset=4
        public int z; // size=0, offset=8
        public short YRot; // size=0, offset=12
        public short Active; // size=0, offset=14
    };

    //Reconstructed, size hint = 8;
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MESH_STRUCT //maybe NODEOFFSET_INFO
    {
        public short unk00; //0
        public short unk01; //2
        public int unk02; //4
        public byte unk03; //8
    };

    public delegate void o_initialise(short item_number);

    public delegate void o_control(short item_number);

    public unsafe delegate void o_floor(ITEM_INFO* item, int x, int y, int z, int* height);

    public unsafe delegate void o_ceiling(ITEM_INFO* item, int x, int y, int z, int* height);

    public unsafe delegate void o_draw_routine(ITEM_INFO* item);

    public unsafe delegate void o_collision(short item_num, ITEM_INFO* laraitem, COLL_INFO* coll);

    public unsafe delegate void o_draw_routine_extra(ITEM_INFO* item);

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct object_info
    {
        public short nmeshes; // size=0, offset=0
        public short mesh_index; // size=0, offset=2
        public int bone_index; // size=0, offset=4
        public short* frame_base; // size=0, offset=8
        public o_initialise initialise;
        public o_control control;
        public o_floor floor;
        public o_ceiling ceiling;
        public o_draw_routine draw_routine;
        public o_collision collision;
        public short object_mip; // size=0, offset=36
        public short anim_index; // size=0, offset=38
        public short hit_points; // size=0, offset=40
        public short pivot_length; // size=0, offset=42
        public short radius; // size=0, offset=44
        public short shadow_size; // size=0, offset=46
        public ushort bite_offset; // size=0, offset=48
        public ushort bitfield;
        public o_draw_routine_extra draw_routine_extra;
        public uint explodable_meshbits; // size=0, offset=56
        public uint padfuck; // size=0, offset=60
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct static_info
    {
        public short mesh_number;
        public short flags;
        public short x_minp;
        public short x_maxp;
        public short y_minp;
        public short y_maxp;
        public short z_minp;
        public short z_maxp;
        public short x_minc;
        public short x_maxc;
        public short y_minc;
        public short y_maxc;
        public short z_minc;
        public short z_maxc;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct room_info
    {
        public short* data; // size=0, offset=0
        public short* door; // size=0, offset=4
        public FLOOR_INFO* floor; // size=8, offset=8
        public LIGHTINFO* light; // size=32, offset=12
        public MESH_INFO* mesh; // size=20, offset=16
        public int x; // size=0, offset=20
        public int y; // size=0, offset=24
        public int z; // size=0, offset=28
        public int minfloor; // size=0, offset=32
        public int maxceiling; // size=0, offset=36
        public short x_size; // size=0, offset=40
        public short y_size; // size=0, offset=42
        public CVECTOR ambient; // size=4, offset=44
        public short num_lights; // size=0, offset=48
        public short num_meshes; // size=0, offset=50
        public byte ReverbType; // size=0, offset=52
        public byte FlipNumber; // size=0, offset=53
        public sbyte MeshEffect; // size=0, offset=54
        public sbyte bound_active; // size=0, offset=55
        public short left; // size=0, offset=56
        public short right; // size=0, offset=58
        public short top; // size=0, offset=60
        public short bottom; // size=0, offset=62
        public short test_left; // size=0, offset=64
        public short test_right; // size=0, offset=66
        public short test_top; // size=0, offset=68
        public short test_bottom; // size=0, offset=70
        public short item_number; // size=0, offset=72
        public short fx_number; // size=0, offset=74
        public short flipped_room; // size=0, offset=76
        public ushort flags; // size=0, offset=78

        public uint Unknown1;
        public uint Unknown2; // Always 0
        public uint Unknown3; // Always 0

        public uint Separator; // 0xCDCDCDCD

        public ushort Unknown4;
        public ushort Unknown5;

        public float RoomX;
        public float RoomY;
        public float RoomZ;

        public fixed uint Separator1[4]; // Always 0xCDCDCDCD
        public uint Separator2; // 0 for normal rooms and 0xCDCDCDCD for null rooms
        public uint Separator3; // Always 0xCDCDCDCD

        public uint NumRoomTriangles;
        public uint NumRoomRectangles;

        public void* Separator4; // Always 0

        public uint LightDataSize;
        public uint NumLights2; // Always same as NumLights

        public uint Unknown6;

        public int RoomYTop;
        public int RoomYBottom;

        public uint NumLayers;

        public tr5_room_layer* LayerOffset;
        public tr5_room_vertex* VerticesOffset;
        public void* PolyOffset;
        public void* PolyOffset2; // Same as PolyOffset

        public uint NumVertices;

        public fixed uint Separator5[4]; // Always 0xCDCDCDCD
    };


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ITEM_INFO
    {
        public int floor; // size=0, offset=0
        public uint touch_bits; // size=0, offset=4
        public uint mesh_bits; // size=0, offset=8
        public short object_number; // size=0, offset=12
        public short current_anim_state; // size=0, offset=14
        public short goal_anim_state; // size=0, offset=16
        public short required_anim_state; // size=0, offset=18
        public short anim_number; // size=0, offset=20
        public short frame_number; // size=0, offset=22
        public short room_number; // size=0, offset=24
        public short next_item; // size=0, offset=26
        public short next_active; // size=0, offset=28
        public short speed; // size=0, offset=30
        public short fallspeed; // size=0, offset=32
        public short hit_points; // size=0, offset=34
        public ushort box_number; // size=0, offset=36
        public short timer; // size=0, offset=38
        public short flags; // size=0, offset=40
        public short shade; // size=0, offset=42
        public short trigger_flags; // size=0, offset=44
        public short carried_item; // size=0, offset=46
        public short after_death; // size=0, offset=48
        public ushort fired_weapon; // size=0, offset=50
        public fixed short item_flags[4]; // size=8, offset=52
        public void* data; // size=0, offset=60
        public PHD_3DPOS pos; // size=20, offset=64
        public ITEM_LIGHT il; // size=48, offset=84

        public uint meshswap_meshbits; // size=0, offset=136 OFF=132
        public short draw_room; // size=0, offset=140 OFF=136
        public short TOSSPAD; // size=0, offset=142 OFF=138
        public fixed sbyte pad1[5472]; // OFF=140
        public uint bitfield;
        public fixed sbyte pad2[8]; // OFF=5614
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct creature_info
    {
        public fixed short joint_rotation[4]; // size=8, offset=0
        public short maximum_turn; // size=0, offset=8
        public short flags; // size=0, offset=10
        public ushort bitfield;
        public mood_type mood; // size=4, offset=14
        public ITEM_INFO* enemy; // size=144, offset=18
        public ITEM_INFO ai_target; // size=144, offset=22
        public short item_num; // size=0, offset=5644
        public PHD_VECTOR target; // size=12, offset=5646
        public lot_info LOT; // size=44, offset=5658
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct tr4_mesh_face3 // 10 bytes
    {
        public fixed ushort Vertices[3];
        public ushort Texture;
        public ushort Effects; // TR4-5 ONLY: alpha blending and environment mapping strength
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct tr4_mesh_face4 // 12 bytes
    {
        public fixed ushort Vertices[4];
        public ushort Texture;
        public ushort Effects;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct tr5_room_layer // 56 bytes
    {
        public uint NumLayerVertices; // Number of vertices in this layer (4 bytes)
        public ushort UnknownL1;
        public ushort NumLayerRectangles; // Number of rectangles in this layer (2 bytes)
        public ushort NumLayerTriangles; // Number of triangles in this layer (2 bytes)
        public ushort UnknownL2;

        public ushort Filler; // Always 0
        public ushort Filler2; // Always 0

        // The following 6 floats define the bounding box for the layer

        public float LayerBoundingBoxX1;
        public float LayerBoundingBoxY1;
        public float LayerBoundingBoxZ1;
        public float LayerBoundingBoxX2;
        public float LayerBoundingBoxY2;
        public float LayerBoundingBoxZ2;

        public uint Filler3; // Always 0 (4 bytes)
        public void* VerticesOffset;
        public void* PolyOffset;
        public void* PolyOffset2;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct tr5_vertex // 12 bytes
    {
        public float x;
        public float y;
        public float z;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct tr5_room_vertex // 28 bytes
    {
        public tr5_vertex Vertex; // Vertex is now floating-point
        public tr5_vertex Normal;
        public uint Colour; // 32-bit colour
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SPRITE_STRUCT // 24 bytes
    {
        public ushort tile;
        public byte x;
        public byte y;
        public ushort width;
        public ushort height;
        public float left;
        public float top;
        public float right;
        public float bottom;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct tr_object_texture_vert // 4 bytes
    {
        public byte Xcoordinate; // 1 if Xpixel is the low value, 255 if Xpixel is the high value in the object texture
        public byte Xpixel;
        public byte Ycoordinate; // 1 if Ypixel is the low value, 255 if Ypixel is the high value in the object texture
        public byte Ypixel;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct tr4_object_texture // 38 bytes
    {
        public ushort Attribute;
        public ushort TileAndFlag;
        public ushort NewFlags;

        public fixed byte Vertices[4 * 4]; // The four corners of the texture

        public uint OriginalU;
        public uint OriginalV;
        public uint Width; // Actually width-1
        public uint Height; // Actually height-1

        public ushort Padding;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct OBJECT_TEXTURE_VERT
    {
        public float x;
        public float y;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct OBJECT_TEXTURE
    {
        public ushort attribute;
        public ushort tile_and_flag;
        public ushort new_flags;
        public fixed byte vertices[4 * 8];
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct DRIP_STRUCT
    {
        public int x; // size=0, offset=0
        public int y; // size=0, offset=4
        public int z; // size=0, offset=8
        public byte On; // size=0, offset=12
        public byte R; // size=0, offset=13
        public byte G; // size=0, offset=14
        public byte B; // size=0, offset=15
        public short Yvel; // size=0, offset=16
        public byte Gravity; // size=0, offset=18
        public byte Life; // size=0, offset=19
        public short RoomNumber; // size=0, offset=20
        public byte Outside; // size=0, offset=22
        public byte Pad; // size=0, offset=23
        public fixed sbyte padding[8];
        public ushort padding1;
        public fixed sbyte padding2[2];
        public ushort padding3;
        public fixed sbyte padding4[2];
    };

    #endregion
}
