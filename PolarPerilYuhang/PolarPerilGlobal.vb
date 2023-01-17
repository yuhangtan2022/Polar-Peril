Module PolarPerilGlobal
    Structure spriteType
        Dim speed As Point
        Dim floatTime As Integer
        Dim picNum As Integer
        Dim faceRight As Boolean
        Dim floating As Boolean
        Dim onLadder As Boolean
        Dim floorNum As Integer
        Dim onFloor As Boolean
        Dim ladderNum As Integer
    End Structure
    Structure floorType
        Dim x As Single
        Dim y As Single
        Dim slope As Single
        Dim leftEdge As Integer
        Dim rightEdge As Integer
    End Structure
    Public quit As Boolean
End Module
