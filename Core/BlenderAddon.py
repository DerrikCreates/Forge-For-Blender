# this file needs god. im not a python dev
# naming and formating might be scuffed

bl_info = {
    "name": "Forge Data Exporter",
    "author": "DerrikCreates",
    "description": "export forge items for latter processing into a .mvar",
    "version": (0.1),
    "blender": (3, 3, 0),
    "location": "View3D",
    "warning": "",
    "category": "Generic",
    "doc_url": "https://github.com/DerrikCreates/Halo-Infinite-Forge-For-Blender"
}

import bpy
import json
from bpy_extras.io_utils import ExportHelper
from bpy.props import StringProperty, BoolProperty, EnumProperty
from bpy.types import Operator


def export_item_data(context, filepath):
    print("Collecting Forge Data...")
    test = ItemData()
    test.rotX = 1

    print(test.toJSON())

    depsgraph = bpy.context.evaluated_depsgraph_get()  # Get ref to the evaulated "scene"?
    objects = depsgraph.scene.objects  # get evaulated objects

    itemList = []
    print("------")
    for object in objects:
        itemData = ItemData()
        evalObject = object.evaluated_get(depsgraph)

        print("checking object " + evalObject.name)
        print(evalObject.data.attributes)
        if len(evalObject.data.attributes) < 2:
            print("----" + evalObject.name + "----")
            if len(evalObject.data.attributes) != 0:
                for attr in evalObject.data.attributes:
                    print(attr.name)

    return {'FINISHED'}


# THIS IS THE EXAMPLE ON GETING ATTRIBUTE DATA OUT OF NODE GRAPH 
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


def CollectData(dataList, filePath):
    active = bpy.context.active_object
    depsGraph = bpy.context.evaluated_depsgraph_get()
    object_eval = active.evaluated_get(depsGraph)

    with open('filePath', 'w') as f:
        for d in dataList:
            if type(d.value) is bpy.types.FloatVectorAttribute:
                print(f'{d} {d.value}')


# print(bpy.data.objects["Cube.003"].modifiers["GeometryNodes"]["Output_3_attribute_name"])


class ItemData:
    rotX = None
    rotY = None
    rotZ = None

    scaleX = None
    scaleY = None
    scaleZ = None

    rotationX = None
    rotationY = None
    rotationZ = None

    def toJSON(self):
        return json.dumps(self, default=lambda o: o.__dict__,
                          sort_keys=True, indent=4)


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
        row.label(text="Hello world!", icon='WORLD_DATA')

        row = layout.row()
        row.label(text="Active object is: " + obj.forge_test_prop)
        row = layout.row()
        row.prop(obj, "forge_test_prop")
        row.prop(obj, "forge_enum")

        row = layout.row()
    # row.operator("mesh.primitive_cube_add")


class ExportSomeData(Operator, ExportHelper):  # panel for exporting data
    """Exporter to save item data for later processing into an mvar format"""
    bl_idname = "halo_forge.save_item_data"  # important since its how bpy.ops.import_test.some_data is constructed
    bl_label = "Export Halo Forge Data"

    # ExportHelper mixin class uses this
    filename_ext = ".txt"

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
    # Both Propertys are from the template they add buttons
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


# Only needed if you want to add into a dynamic menu
def menu_func_export(self, context):
    self.layout.operator(ExportSomeData.bl_idname, text="Export Halo Forge Data")


# Register and add to the "file selector" menu (required to use F3 search "Text Export Operator" for quick access).
def register():
    bpy.utils.register_class(ExportSomeData)
    bpy.utils.register_class(ForgeItemPropertiesPanel)
    bpy.types.TOPBAR_MT_file_export.append(menu_func_export)

    # Propertys
    bpy.types.Object.forge_test_prop = bpy.props.StringProperty(
        name='Object scoped prop test',
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
    bpy.types.TOPBAR_MT_file_export.remove(menu_func_export)


if __name__ == "__main__":
    register()

    # test call
    bpy.ops.halo_forge.save_item_data('INVOKE_DEFAULT')
