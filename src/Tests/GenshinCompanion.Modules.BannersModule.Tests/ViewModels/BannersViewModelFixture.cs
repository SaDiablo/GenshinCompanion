using GenshinCompanion.Modules.BannersModule.ViewModels;
using GenshinCompanion.Services.Interfaces;
using Moq;
using Prism.Regions;
using Xunit;

namespace GenshinCompanion.Modules.BannersModule.Tests.ViewModels
{
    public class CharacterBannerViewModelFixture
    {
        private readonly Mock<IMessageService> _messageServiceMock;
        private readonly Mock<IRegionManager> _regionManagerMock;
        //private const string MessageServiceDefaultMessage = "Some Value";

        public CharacterBannerViewModelFixture()
        {
            var messageService = new Mock<IMessageService>();
            //messageService.Setup(x => x.GetMessage()).Returns(MessageServiceDefaultMessage);
            _messageServiceMock = messageService;

            _regionManagerMock = new Mock<IRegionManager>();
        }

        [Fact]
        public void MessagePropertyValueUpdated()
        {
            var vm = new BannersViewModel(_regionManagerMock.Object, _messageServiceMock.Object);

            //_messageServiceMock.Verify(x => x.GetMessage(), Times.Once);

            //Assert.Equal(MessageServiceDefaultMessage, vm.Message);
        }

        [Fact]
        public void MessageINotifyPropertyChangedCalled()
        {
            var vm = new BannersViewModel(_regionManagerMock.Object, _messageServiceMock.Object);
            //Assert.PropertyChanged(vm, nameof(vm.Message), () => vm.Message = "Changed");
        }
    }
}