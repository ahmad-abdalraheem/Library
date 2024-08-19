using Application.Repository;
using Application.Service;
using Domain.Entities;
using Infrastructure.FileModule;
using Moq;

namespace ConsoleApp.Tests
{
    public class MembersScreenTests
    {
        private readonly Mock<MemberService> _mockMemberService;
        private readonly TestConsole _testConsole = new TestConsole();
        private readonly MembersScreen _membersScreen;
        private List<Member> _membersList = new List<Member>();
        public MembersScreenTests()
        {
            _mockMemberService = new Mock<MemberService>(new MemberRepository(new MemberHandler()));
            _membersScreen = new MembersScreen(_mockMemberService.Object, _testConsole);
        }

        private bool SearchInOutput(string searchString)
        {
            foreach (string line in _testConsole.Output)
            {
                if(line.Contains(searchString))
                    return true;
            }
            return false;
        }
        
        [Fact]
        public void MembersMenu_ShouldDisplayErrorMessage_WhenMembersListIsNull()
        {
            // Arrange
            _mockMemberService.Setup(s => s.Get()).Returns((List<Member>?)null);

            // Act
            _testConsole.AddKeySequence([ConsoleKey.Enter]);
            int result = _membersScreen.MembersMenu();
    
            // Assert
            Assert.True(SearchInOutput("Error While loading Data"));
            Assert.Equal(0, result);
        }

        [Fact]
        public void MembersMenu_ShouldDisplayNoMembersMessage_WhenMembersListIsEmpty()
        {
            // Arrange
            _mockMemberService.Setup(s => s.Get()).Returns(new List<Member>());
            _testConsole.KeyInput.Clear();
            _testConsole.AddKeySequence([ConsoleKey.DownArrow, ConsoleKey.Enter, ConsoleKey.DownArrow, ConsoleKey.DownArrow, ConsoleKey.DownArrow, ConsoleKey.Enter]);
            
            // Act
            int result = _membersScreen.MembersMenu();
            
            // Assert
            Assert.True(SearchInOutput("No members found"));
            Assert.True(SearchInOutput("Add a new Member"));
            Assert.True(SearchInOutput("Back to main menu."));
            Assert.Equal(0, result);
        }
        
        [Fact]
        public void MembersMenu_ShouldAddMemberToList_WhenMembersListIsEmpty()
        {
            // Arrange
            _membersList = new List<Member>();
            _mockMemberService.Setup(s => s.Get()).Returns(_membersList);
            _mockMemberService.Setup(ms => ms.Add(It.IsAny<Member>())).Callback((Member member) => _membersList.Add(member)).Returns(true);
            _testConsole.KeyInput.Clear();
            _testConsole.AddKeySequence([ConsoleKey.Enter, ConsoleKey.Backspace]);
            _testConsole.AddInputSequence(["Member 1", "Email 1"]);
            
            // Act
            int result = _membersScreen.MembersMenu();
            
            Assert.Single(_membersList);
            Assert.Equal(0, result);
        }
        
        [Fact]
        public void MembersMenu_ShouldDisplayMembersAndMoveBetweenMembers()
        {
            // Arrange
            _membersList = new List<Member>()
            {
                new Member() {Id = 1, Name = "Mami", Email = "Mami@gmail.com"},
                new Member() {Id = 2, Name = "Mami2", Email = "Mami2@gmail.com"}
            };
            _mockMemberService.Setup(s => s.Get()).Returns(_membersList);
            _testConsole.KeyInput.Clear();
            _testConsole.AddKeySequence([ConsoleKey.DownArrow, ConsoleKey.UpArrow, ConsoleKey.DownArrow, ConsoleKey.Backspace]);
            
            // Act
            int result = _membersScreen.MembersMenu();
            
            Assert.Equal(2, _membersList.Count);
            Assert.Equal(0, result);
        }
        
        [Fact]
        public void MembersMenu_ShouldDeleteSelectedMember_WhenDeleteKeyIsPressed()
        {
            // Arrange
            _membersList = new List<Member>()
            {
                new Member() {Id = 1, Name = "Mami", Email = "Mami@gmail.com"},
                new Member() {Id = 2, Name = "Mami2", Email = "Mami2@gmail.com"}
            };
            _mockMemberService.Setup(s => s.Get()).Returns(_membersList);
            _mockMemberService.Setup(ms => ms.Delete(It.IsAny<int>())).Callback((int id) => _membersList.RemoveAll(m => m.Id == id));
                
            _testConsole.KeyInput.Clear();
            _testConsole.AddKeySequence([ConsoleKey.DownArrow, ConsoleKey.UpArrow, ConsoleKey.Delete, ConsoleKey.Backspace]);
            
            // Act
            int result = _membersScreen.MembersMenu();
            
            Assert.Single(_membersList);
            Assert.Equal(0, result);
        }
        
        [Fact]
        public void MembersMenu_ShouldUpdateSelectedMember_WhenEnterKeyIsPressed()
        {
            // Arrange
            _membersList = new List<Member>()
            {
                new Member() {Id = 1, Name = "Mami", Email = "Mami@gmail.com"},
                new Member() {Id = 2, Name = "Mami2", Email = "Mami2@gmail.com"}
            };
            _mockMemberService.Setup(s => s.Get()).Returns(_membersList);
            _mockMemberService.Setup(ms => ms.Update(It.IsAny<Member>())).Callback((Member member) =>
            {
                int index = _membersList.FindIndex(m => m.Id == member.Id);
                _membersList[index].Name = member.Name;
                _membersList[index].Email = member.Email;
            }).Returns(true);
            
            _testConsole.KeyInput.Clear();
            _testConsole.AddInputSequence(["Member 1", ""]);
            _testConsole.AddKeySequence([ConsoleKey.Enter, ConsoleKey.Backspace]);
            
            // Act
            int result = _membersScreen.MembersMenu();
            
            // Assert
            Assert.Equal("Member 1", _membersList[0].Name);
            Assert.Equal("Mami@gmail.com", _membersList[0].Email);
            Assert.Equal(2, _membersList.Count);
            Assert.Equal(0, result);
        }

        [Fact]
        public void MembersMenu_ShouldAddMemberToList_WhenAddKeyIsPressed()
        {
            _membersList = new List<Member>()
            {
                new Member() {Id = 1, Name = "Mami", Email = "Mami@gmail.com"},
                new Member() {Id = 2, Name = "Mami2", Email = "Mami2@gmail.com"}
            };
            _mockMemberService.Setup(s => s.Get()).Returns(_membersList);
            _mockMemberService.Setup(ms => ms.Add(It.IsAny<Member>())).Callback((Member member) => _membersList.Add(member)).Returns(true);
            _testConsole.KeyInput.Clear();
            _testConsole.AddInputSequence(["", "   ", "Member 1", "Email 1"]);
            _testConsole.AddKeySequence([ConsoleKey.Add, ConsoleKey.Backspace]);
            
            // Act
            int result = _membersScreen.MembersMenu();
            
            Assert.Equal(3, _membersList.Count);
            Assert.Equal(0, result);
        }
        
        [Fact]
        public void SearchInOutput_ShouldReturnFalse_WhenKeywordNotExist()
        {
            Assert.False(SearchInOutput("Ahmad Abdelkareem Nairat"));
        }
    }
}
