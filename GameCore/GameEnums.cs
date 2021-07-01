using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGLTutorial.GameCore
{
    /// <summary>
    /// Enum für die drei Achsen des Koordinatensystems.
    /// </summary>
    public enum Axis
    {
        X,
        Y,
        Z
    }

    // Hinweis:
    // Enums sind sinnvoll, wenn man statt Strings lieber Zahlen verwenden will und trotzdem
    // nicht auf sprechende Bezeichnungen verzichten will.
    // Mit einem int könnte man die Achsen X, Y und Z z.B. nur als 0, 1 und 2 kodieren.
    // Da das nicht intuitiv ist, kann man ein enum definieren, das dann wie ein Datentyp (int, double, etc.)
    // eingesetzt werden kann. Eine Variable des Typs Axis kann dann nur einen dieser drei Werte annehmen.
    // Intern sind enums in der Regel Zahlenwerte (und somit schneller als strings).
}
