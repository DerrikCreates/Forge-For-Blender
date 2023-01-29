# this file needs god. im not a python dev
# naming and formating might be scuffed

bl_info = {
    "name": "Forge Data Exporter",
    "author": "DerrikCreates.com",
    "description": "export forge items for latter processing into a .mvar",
    "blender": (3, 3, 0),
    "version": (0, 1, 0),
    "location": "View3D",
    "warning": "",
    "category": "Generic"
}

import bpy
import json
from types import SimpleNamespace
from typing import List
from bpy_extras.io_utils import ExportHelper
from mathutils import Vector
from bpy.props import StringProperty, BoolProperty, EnumProperty
from bpy.types import Operator

from bpy.types import (
    Panel,
    UIList,
)

from rna_prop_ui import PropertyPanel


def export_point_cloud(context, filepath):
    print("Collecting Forge Data...")

    depsgraph = bpy.context.evaluated_depsgraph_get()  # Get ref to the evaulated "scene"?
    objects = depsgraph.scene.objects  # get evaulated objects
    bpy.data.objects["PrimitiveCube-4x4x4.010"]

    foundMapSettings = False
    mapSettingsObject = None
    itemList = []
    print("------")
    for object in objects:
        print(object)
        print(f"{object.forge_isMapSettings} map settings bool")

        if object.forge_isMapSettings == True:

            if foundMapSettings == True:
                print(
                    f"{mapSettingsObject.name} and {object.name} are both map settings objects. Remove one or unflag it in Object Properties")

                raise TypeError(
                    f"{mapSettingsObject.name} and {object.name} are both map settings objects. Remove one or unflag it in Object Properties")
                return
            foundMapSettings = True
            mapSettingsObject = object 

    mapData = MapData()

    activeObject = context.active_object
    activeObject = activeObject.evaluated_get(depsgraph)
    verts = activeObject.data.vertices
    itemList = []

    for vert in verts:
        # CLOWN SHIT REMOVE LATER
        ttt = bpy.data.objects.new(name="test", object_data=bpy.data.objects["PrimitiveCube-4x4x4.001"].data)
        bpy.data.collections["CLOWN"].objects.link(ttt)

        # end clown shit
        print(vert.normal)

        exportItem = ItemData()
        pos = activeObject.matrix_world @ vert.co

        # CLOWN SHIT
        ttt.location[0] = pos.x
        ttt.location[1] = pos.y
        ttt.location[2] = pos.z
        # END CLOWN
        exportItem.positionX = pos.x
        exportItem.positionY = pos.y
        exportItem.positionZ = pos.z

        exportItem.scaleX = 0.5
        exportItem.scaleY = 0.5
        exportItem.scaleZ = 0.5

        exportItem.rotationX = vert.normal.x
        exportItem.rotationY = vert.normal.y
        exportItem.rotationZ = vert.normal.z

        exportItem.itemId = activeObject.forge_object_id
        print(exportItem)

        itemList.append(exportItem)

    print("------")

    mapData.itemList = itemList
    mapData.mapId = mapSettingsObject.forge_mapId_enum

    file = open(filepath, 'w')

    file.write(mapData.toJSON())  # todo extract json writing to method
    print(f"File saved to {filepath}")
    return {'FINISHED'}


def export_item_data(context, filepath):
    print("Collecting Forge Data...")

    depsgraph = bpy.context.evaluated_depsgraph_get()  # Get ref to the evaulated "scene"?
    objects = depsgraph.scene.objects  # get evaulated objects

    
    mapData = MapData()
    
    
    itemList = []
    print("------")
    for object in objects:
        
        if object.forge_object_id == 0:
            print(f"{object.name} does not have a valid object id skipping export")
            continue
            
        if object.forge_export_toggle == True:

            itemData = ItemData()
            evalObject = object.evaluated_get(depsgraph)

            print("checking object " + evalObject.name)

            #  if len(evalObject.data.attributes) < 2:
            #     print( "----"+ evalObject.name + "----")
            #    if len(evalObject.data.attributes) != 0:
            #        for attr in evalObject.data.attributes:
            #              print(attr.name)

            # TODO add a ctor to the class and remove all this bs
            
            quat = evalObject.rotation_euler.to_quaternion()
            itemData.qX = quat.x
            itemData.qY = quat.y
            itemData.qZ = quat.z
            itemData.qW = quat.w
            
            pos = evalObject.location
            itemData.positionX = pos.x
            itemData.positionY = pos.y
            itemData.positionZ = pos.z

            rot = evalObject.rotation_euler
            itemData.rotationX = rot.x
            itemData.rotationY = rot.y
            itemData.rotationZ = rot.z

            if evalObject.forge_use_dimensions_toggle:
                scale = evalObject.dimensions
            else:
                scale = evalObject.scale

            itemData.scaleX = scale.x
            itemData.scaleY = scale.y
            itemData.scaleZ = scale.z

            forwardVector = evalObject.matrix_world.to_quaternion() @ Vector((1.0, 0.0, 0.0))  ##1 1   -1 1   -1 -1   1 -1
            forwardVector = forwardVector.normalized()
            itemData.forwardX = forwardVector.x
            itemData.forwardY = forwardVector.y
            itemData.forwardZ = forwardVector.z

            upVector = evalObject.matrix_world.to_quaternion() @ Vector((0.0, 0.0, 1.0))  ##-1.0))
            upVector = upVector.normalized()
            
            itemData.upX = upVector.x
            itemData.upY = upVector.y
            itemData.upz = upVector.z

            print(evalObject.forge_object_id)
            itemData.itemId = evalObject.forge_object_id

            print(itemData.testList)

            itemList.append(itemData)

            mapData.itemList = itemList
        else:
            print(f"Skipping {object.name} because its not marked for export")

    mapData.mapId = bpy.context.scene.forge_mapId_enum

    print(mapData.mapId)
    file = open(filepath, 'w')
    file.write(mapData.toJSON())
    print(f"File saved to {filepath}")
    return {'FINISHED'}

    # THIS IS THE EXAMPLE ON GETING ATTRIBUTE DATA OUT OF NODE GRAPH 
    # just make sure to get the evaluated object


# basePath = "G:/__Inbox/"
# data = object_eval.data
# CollectData(data.attributes['ID'].data,
#             basePath + "ID.txt")

#  CollectData(data.attributes['InstancePostion'].data,
#              basePath + "Postion.txt")

#  CollectData(data.attributes['InstanceRotation'].data,
#             basePath + "Rotation.txt")

# f = open(filepath, 'w', encoding='utf-8')
# f.write("Hello World %s" % use_some_setting)
# f.close()

# get up and forward rotation
# up = obj.matrix_world.to_quaternion() @ Vector((0.0, 1.0, 0.0))
# forward = obj.matrix_world.to_quaternion() @ Vector((0.0, 0.0, 1.0))


# print(bpy.data.objects["Cube.003"].modifiers["GeometryNodes"]["Output_3_attribute_name"])


class ItemData:

    def __init__(self):
        self.testList = []

    testList = None

    qX = None
    qY = None
    qZ = None
    qW = None
    
    positionX = None
    positionY = None
    positionZ = None

    scaleX = None
    scaleY = None
    scaleZ = None

    rotationX = None
    rotationY = None
    rotationZ = None

    forwardX = None
    forwardY = None
    forwardZ = None

    upX = None
    upY = None
    upZ = None

    itemId = None

    # def toJSON(self):
    #    return json.dumps(self, default=lambda o: o.__dict__,
    #                     sort_keys=True, indent=4)


class MapData:
    mapId = None

    itemList: List[ItemData] = []

    def toJSON(self):
        return json.dumps(self, default=lambda o: o.__dict__,
                          sort_keys=True, indent=None)


class ForgeMapPanel(Panel):
    bl_space_type = 'PROPERTIES'
    bl_region_type = 'WINDOW'
    bl_context = "scene"
    bl_label = "Forge Map Settings"

    def draw(self, context):
        scene = context.scene
        layout = self.layout
        row = layout.row()
        row.prop(scene, "forge_mapId_enum")


class ForgeItemPropertiesPanel(bpy.types.Panel):
    """Creates a Panel in the Object properties window"""
    bl_label = "Forge Item Properties"
    bl_idname = "OBJECT_PT_forge_item"
    bl_space_type = 'PROPERTIES'
    bl_region_type = 'WINDOW'
    bl_context = "object"

    def draw(self, context):
        layout = self.layout

        obj = context.object

        row = layout.row()
        row.label(text="Halo Forge Exporter Item Properites", icon='PROPERTIES')

        row = layout.row()
        # row.label(text="Active object is: " + obj.forge_test_prop)
        row = layout.row()
        # row.prop(obj, "forge_test_prop")
        # row.prop(obj, "forge_enum")

        row.prop(obj, 'forge_export_toggle')

        self.layout.operator('halo_forge.dynamic_object_lock')
        row = layout.row()
        row.label(text="Map Settings (ONLY ONE OBJECT ALLOWED)", icon='PROPERTIES')
        row = layout.row()
        row.prop(obj, "forge_isMapSettings")
        row = layout.row()
        row.prop(obj, "forge_mapId_enum")

        row = layout.row()
        row.label(text="***DANGER ZONE!*** DO NOT CHANGE ANYTHING BELOW")
        row = layout.row()
        row.label(icon='ERROR')
        row.label(icon='ERROR')
        row.label(icon='ERROR')
        row.label(icon='ERROR')
        row.label(icon='ERROR')
        row.label(icon='ERROR')
        row.label(icon='ERROR')
        row.label(icon='ERROR')

        row = layout.row()
        row.prop(obj, "forge_object_id")
        row = layout.row()
        row.prop(obj, "forge_object_variant_id")
        row = layout.row()
        row.prop(obj, "forge_use_dimensions_toggle")

    # row.operator("mesh.primitive_cube_add")


class ExportSomeData(Operator, ExportHelper):  # panel for exporting data
    """Exporter to save item data for later processing into an mvar format"""
    bl_idname = "halo_forge.save_item_data"  # important since its how bpy.ops.import_test.some_data is constructed
    bl_label = "Export Halo Forge Data"

    # ExportHelper mixin class uses this
    filename_ext = ".DCjson"

    filter_glob: StringProperty(
        default="*.txt",
        options={'HIDDEN'},
        maxlen=255,  # Max internal buffer length, longer would be clamped.
    )

    # List of operator properties, the attributes will be assigned
    # to the class instance from the operator settings before calling.
    use_setting: BoolProperty(
        name="Example Boolean",
        description="Example Tooltip",
        default=True,
    )
    # Both are from the template they add buttons
    # on the side of the export window
    type: EnumProperty(
        name="Example Enum",
        description="Choose between two items",
        items=(
            ('OPT_A', "First Option", "Description one"),
            ('OPT_B', "Second Option", "Description two"),
        ),
        default='OPT_A',
    )

    def execute(self, context):
        return export_item_data(context, self.filepath)


class ExportPointCloudData(Operator, ExportHelper):
    """Export point cloud to Halo Forge"""
    bl_idname = "halo_forge.save_point_cloud"  # important since its how bpy.ops.import_test.some_data is constructed
    bl_label = "Export Halo Point Cloud"
    filename_ext = ".DCjson"

    def execute(self, context):
        return export_point_cloud(context, self.filepath)


class DynamicObjectLock(bpy.types.Operator):
    bl_idname = "halo_forge.dynamic_object_lock"
    bl_label = "Make Object Dynamic"

    def execute(self, context):
        print(context)
        return {'FINISHED'}


class ExportItemToForge(bpy.types.Operator):
    bl_idname = "halo_forge.export_item_to_forge"  # important since its how bpy.ops.import_test.some_data is constructed
    bl_label = "Export Item To Forge"

    def execute(self, context):
        print("ExportItemToForge Operator ran")


# Only needed if you want to add into a dynamic menu
def menu_func_export(self, context):
    self.layout.operator(ExportSomeData.bl_idname, text="Export Halo Forge Data")
    self.layout.operator(ExportPointCloudData.bl_idname, text="Export Forge Point Cloud")



class LoadForgeObejcts(bpy.types.Operator):
    bl_idname = "halo_forge.load_forge_objects"
    bl_label = "Load forge objects DEV ONLY"

    def execute(self, context):
        rootPath= "Z:/Halo/HaloData/__chore/gen__/"
        f = open("Z:/Halo/ItemData.json","r")
        data = f.read()
        f.close()
        obj = json.loads(data, object_hook=lambda d: SimpleNamespace(**d))
        print(obj[1])
        
       # for x in obj:
            
        filepath =  filepath=obj[1].filePath;
        
        
        
        print(f"Z:/Halo/HaloData/__chore/gen__/{filepath}")
        
       # bpy.ops.infinite.rendermodel(f"Z:/Halo/HaloData/__chore/gen__{filepath}", auto_import_dependencies=False, import_uvs=True, import_weights=True, import_normals=True, reuse_textures=True, add_materials=True, populate_shader=False, import_model=True, use_modules=False, mipmap=0, norm_signed=False, lod=0, scale_modifier=(1, 1, 1))
        
        return {'FINISHED'}
    
class MassImportHaloAssets(bpy.types.Operator):
    bl_idname = "forge.mass_import"
    bl_label = "MassImportHaloAssets"
    
    def execute(self, context):
        # Implement your first function here
        
         
        f = open("G:\\__Inbox\\terrainTest.json")
        
        jsonStr = f.read()

       # print(jsonStr)

        aList = json.loads(jsonStr)

        for item in aList:
            print(f"{item['id']} {item['path']}")
            bpy.ops.infinite.rendermodel(filepath="", auto_import_dependencies=False, import_uvs=True,
 import_weights=True, import_normals=True, reuse_textures=True, add_materials=True, populate_shader=False,
 import_model=True, use_modules=False, mipmap=0, norm_signed=False, lod=0, scale_modifier=(1, 1, 1))
            
        
        f.close()
        
        return {'FINISHED'}
    
    
    
    
class PlaceItemsOnVerts(bpy.types.Operator):
    bl_idname = "forge.items_to_verts"
    bl_label = "Forge Items To Verticies"
    
    def execute(self, context):
        depsgraph = context.evaluated_depsgraph_get()
        obj = context.object
        
        active = context.object
        selected = context.selected_objects
        item_to_copy = None
        
        last_index = len(selected) - 1
        if last_index != 1:
            print("select 2 items")
            return {'FINISHED'}
        
        for object in selected:
            if active != object:
                item_to_copy = object
                
        
        print(f"The item to copy is: {item_to_copy}")
          
          
              
        
        print(f"{active} is the item to copy to")
        print(f"{item_to_copy} is the item to copy")           
        object_eval = obj.evaluated_get(depsgraph)
        
       # v =  object_eval.data.vertices[0]
        collection = bpy.data.collections.new(name="forge point cloud")
        scene_collection = context.scene.collection
        
        
        scene_collection.children.link(collection)
        for vert in object_eval.data.vertices:
                
            new_item = item_to_copy.copy()
            collection.objects.link(new_item)
            matrix = active.matrix_world    
            new_item.location = matrix @ vert.co
        
        return {'FINISHED'}
    
class HideNonForgeObjects(bpy.types.Operator):
        bl_idname = "forge.hide_nonforge_objects"
        bl_label = "Hide Nonforge Objects"
        def execute(self, context):
            for obj in bpy.data.objects:
                if obj.forge_object_id != 0:
                    continue
                obj.hide_set(True)
            return {'FINISHED'}
        
class ShowNonForgeObjects(bpy.types.Operator):
        bl_idname = "forge.show_nonforge_objects"
        bl_label = "Show Nonforge Objects"
        def execute(self, context):
            for obj in bpy.data.objects:
                if obj.forge_object_id != 0:
                    continue
                obj.hide_set(False)
            return {'FINISHED'}
        
class ShowForgeObjects(bpy.types.Operator):
        bl_idname = "forge.show_forge_objects"
        bl_label = "Show forge Objects"
        def execute(self, context):
            for obj in bpy.data.objects:
                if obj.forge_object_id == 0:
                    continue
                obj.hide_set(False)
            return {'FINISHED'}
        
class HideForgeObjects(bpy.types.Operator):
        bl_idname = "forge.hide_forge_objects"
        bl_label = "Hide Forge Objects"
        def execute(self, context):
            for obj in bpy.data.objects:
                if obj.forge_object_id == 0:
                    continue
                obj.hide_set(True)
            return {'FINISHED'}
    
# Register and add to the "file selector" menu (required to use F3 search "Text Export Operator" for quick access).
def register():
    bpy.utils.register_class(HideForgeObjects)
    bpy.utils.register_class(ShowForgeObjects)
    bpy.utils.register_class(ShowNonForgeObjects)
    bpy.utils.register_class(HideNonForgeObjects)
    bpy.utils.register_class(PlaceItemsOnVerts)
    bpy.utils.register_class(MassImportHaloAssets)
    bpy.utils.register_class(ExportSomeData)
    bpy.utils.register_class(ExportPointCloudData)
    bpy.utils.register_class(ForgeItemPropertiesPanel)
    bpy.utils.register_class(DynamicObjectLock)
    bpy.utils.register_class(ExportItemToForge)
    bpy.utils.register_class(ForgeMapPanel)
    bpy.utils.register_class(LoadForgeObejcts)
    bpy.types.TOPBAR_MT_file_export.append(menu_func_export)

    # Properties
    bpy.types.Object.forge_test_prop = bpy.props.StringProperty(
        name='Object scoped prop test',
    )

    bpy.types.Object.forge_export_toggle = bpy.props.BoolProperty(
        name="Export Item",
        description="This enables this item to be saved when exporting",
        default=True,

    )
    bpy.types.Object.forge_use_dimensions_toggle = bpy.props.BoolProperty(
        name="Export Dimension",
        description="Exports dimensions instead of scale",
        default=False,

    )
    bpy.types.Object.forge_isStatic = bpy.props.BoolProperty(
        name="Is Static",
        description="Says if this object is static or not",
        default=True,

    )

    ### MAP SETTINGS
    bpy.types.Object.forge_isMapSettings = bpy.props.BoolProperty(
        name="Map Settings Object",
        description="Only one should exist in a scene",
        default=False,

    )

    bpy.types.Object.forge_mapId = bpy.props.IntProperty(
        name="Map ID",
        description="Halo Map ID. Only Works on THE map settings object",
    )

    bpy.types.Scene.forge_mapId_enum = bpy.props.EnumProperty(
        name="",
        description="Map ID",
        items=[
            ('-1598071734', "AQUARIUS", "Test 1 Tooltip desc"),
            ('-1449092339', "BEHEMOTH", "Test 2 Tooltip desc"),
            ('847557134', "BREAKER", "Test 3 Tooltip desc"),
            ('-1044063363', "CATALYST", "Test 3 Tooltip desc"),
            ('-340635692', "FRAGMENTATION", "Test 3 Tooltip desc"),
            ('-2109972058', "HIGH_POWER", "Test 3 Tooltip desc"),
            ('-738424322', "LAUNCH_SITE", "Test 3 Tooltip desc"),
            ('1253388187', "LIVE_FIRE", "Test 3 Tooltip desc"),
            ('-687782121', "RECHARGE", "Test 3 Tooltip desc"),
            ('-785503777', "DEADLOCK", "Test 3 Tooltip desc"),
        ]
    )

    ## MAP SETTINGS END

    bpy.types.Object.forge_object_id = bpy.props.IntProperty(

        name="Object ID",
        description="Do not change this unless you know what you are doing!",
    )

    bpy.types.Object.forge_object_variant_id = bpy.props.IntProperty(

        name="Variant ID",
        description="Do not change this unless you know what you are doing!",
    )
    bpy.types.Object.forge_enum = bpy.props.EnumProperty(
        name="",
        description="Forge Enum Prop",
        items=[
            ('OP1', "Option 1 Name", "Test 1 Tooltip desc"),
            ('OP2', "Option 2 Name", "Test 2 Tooltip desc"),
            ('OP2', "Option 3 Name", "Test 3 Tooltip desc"),
        ]
    )


def unregister():
    bpy.utils.unregister_class(ExportSomeData)
    bpy.utils.unregister_class(ForgeItemPropertiesPanel)
    bpy.utils.unregister_class(DynamicObjectLock)
    bpy.types.TOPBAR_MT_file_export.remove(menu_func_export)


if __name__ == "__main__":
    register()

    # test call
    bpy.ops.halo_forge.save_item_data('INVOKE_DEFAULT')



class ForgeObjectDataTagFile():
    ItemId = None
    filePath = None