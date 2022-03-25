namespace Core.Maybe;

public static class MaybeString
{
  public static string OrEmpty(this Maybe<string> maybe) => maybe.OrElse(string.Empty);
}