Module Utils
    Function NormaliseVector(x As Double, y As Double) As Double()
        Dim res(2) As Double
        Dim magnitude As Double = (Math.Sqrt(x * x + y * y))

        If (magnitude <> 0) Then
            x = x / magnitude
            y = y / magnitude
        End If

        res(0) = x
        res(1) = y

        Return res
    End Function

    Function NormalisePointFVector(vec As PointF) As PointF
        Dim res As PointF
        Dim magnitude As Double = (Math.Sqrt(vec.X * vec.X + vec.Y * vec.Y))

        If (magnitude <> 0) Then
            res = New PointF(
                vec.X / magnitude,
                vec.Y / magnitude
            )
        End If

        Return res
    End Function


End Module
