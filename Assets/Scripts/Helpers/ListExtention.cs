using System.Collections.Generic;


public static class ListExtensions
{
  public static T lastOrDefault<T>( this IList<T> list )
  {
    int list_count = list.Count;
    if ( list_count > 0 )
      return list[list_count - 1];

    return default;
  }
}
