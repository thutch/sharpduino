namespace Sharpduino.Library.Tests.Creators
{
    public abstract class BaseMessageCreatorTest
    {
        public abstract void Creates_Appropriate_Message();
        public abstract void Throws_Error_On_Wrong_Message();
    }
}