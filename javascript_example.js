var family = [];
var counter = 0;

window.onload = function() {
  displayFamilySection();
  addButton();
  submitButton();
}

function addButton() {
  document.getElementsByTagName('button')[0].addEventListener("click", function(event) {
    var form = document.querySelector('form');
    updateFamilySection(event, form);
  });
}

function submitButton() {
  document.getElementsByTagName('button')[1].addEventListener("click", function(event) {
    submitFamily(event);
  });
}

function updateFamilySection(event, form) {
  if (validateFields(event, form[0].value, form[1].value)) {
    addFamilyMember(event, form);
    counter++;
  }
  document.querySelector('form').reset();
}

function addFamilyMember(event, form) {
  event.preventDefault();
  if (event.target.className.includes('submitted')) {
    counter = 0;
    var listItems = document.getElementById('family_members').children
    for (var i = 0; i < listItems.length; i++) {
      if (listItems[i].nodeName == 'UL') {
        counter++;
      }
    }
  }
  var newFamilyMember = createFamilyMember(form);
  family.push(newFamilyMember);
  appendMemberToFamilySection(newFamilyMember);
}

function createFamilyMember(form) {
  var familyMember = {}
  familyMember["age"] = form[0].value
  familyMember["rel"] = form[1].value
  familyMember["smoker"] = form[2].checked ? 'yes' : 'no';
  return familyMember;
}

function displayFamilySection() {
  var section = document.getElementsByClassName('debug')[0];
  section.style.display = 'block';
  createList(section, 'family_members');
}

function createList(list, id) {
  var ul = document.createElement('ul');
  ul.setAttribute('id', id);
  list.appendChild(ul);
  return ul;
}

function submitFamily(event) {
  event.preventDefault();
  if (event.target.type == "submit" && (document.getElementById('family_members').children[0].innerHTML != "Make Changes")) {
    makeChangesButton(event);
  }
  document.getElementsByTagName('button')[0].className += ' submitted';
  var familyJSON = JSON.stringify(family);
}

function makeChangesButton(event) {
  event.preventDefault();
  var list = document.getElementsByClassName('debug')[0].children[0];
  var makeChanges = document.createElement('button');
  makeChanges.innerHTML = 'Make Changes';
  makeChanges.id = "make_changes";
  makeChanges.type = 'button';
  makeChanges.onclick = function() {
    showChangeButtons('remove');
    showChangeButtons('edit');
  }
  list.prepend(makeChanges);
}

function showChangeButtons(option) {
  var option = document.getElementsByClassName(`${option}_button`);
  for (var i in option) {
    option[i].type = 'button';
  }
}

function appendMemberToFamilySection(familyMember) {
  var person = createList(document.getElementById('family_members'), counter);
  var memberKeys = Object.keys(familyMember);
  personHeader(person);
  appendButtons(person);
  appendAttributes(person, memberKeys, familyMember);
}

function personHeader(person){
  var header = document.createElement('p');
  var listItems = document.getElementById('family_members').children
  for (var i = 0; i < listItems.length; i++) {
    if (listItems[i].nodeName == 'UL') {
      var id = parseInt(listItems[i].id) + 1;
      header.innerHTML = `Family Member #${id}`
    }
  }
  person.appendChild(header);
}

function appendButtons(person) {
  var removeButton = createButton('remove', person.id);
  var editButton = createButton('edit', person.id);
  var saveButton = createButton('save', person.id);
  if (event.target.className.includes('submitted')) {
    removeButton.type = 'button';
    editButton.type = 'button';
  }
  person.appendChild(removeButton);
  person.appendChild(editButton);
  person.appendChild(saveButton);
  person.appendChild(document.createElement('br'));
}

function appendAttributes(person, memberKeys, familyMember) {
  for (var i = 0; i < memberKeys.length; i++) {
    var newLi = document.createElement('li');
    newLi.innerHTML = memberKeys[i] + ": " + '<span class="attribute">' + familyMember[memberKeys[i]] + '</span>';
    person.appendChild(newLi);
  }
}

function validateFields(event, age, rel) {
  event.preventDefault();
  if (!parseFloat(age) || age == "" || age < 1) {
    alert('Age is required');
  } else if (rel == "") {
    alert('Relationship is required');
  } else {
    return true;
  }
}

function createButton(purpose, id) {
  var element = document.createElement("input");
  element.type = "hidden";
  element.value = `${purpose} family member`;
  element.className = `${purpose}_button`;
  element.onclick = function() {
    if (purpose == 'remove') {
      removeFamilyMember(id);
    } else if (purpose == 'edit') {
      editFamilyMember(id);
    }
  };
  return element;
}

function removeFamilyMember(id) {
  document.getElementById(id).remove();
  counter--;
  updateList();
}

function updateList() {
  var listItems = document.getElementById('family_members').children;
  if (document.getElementById('family_members').innerHTML.includes('ul')) {
    for (var i = 0; i < listItems.length; i ++) {
      if (listItems[i].nodeName == 'UL') {
        listItems[i].getElementsByTagName('p')[0].innerHTML = `Family Member #${i}`
      }
    }
  } else {
    document.getElementById('make_changes').style.display = 'none';
  }
}

function editFamilyMember(id) {
  document.getElementsByClassName('edit_button')[id].type = 'hidden';
  var member = document.getElementById(id);
  hideAttributes(member.getElementsByTagName('li'));

  var newForm = document.getElementsByClassName('builder')[0].children[1].cloneNode(true);
  newForm.children[4].remove()
  newForm.setAttribute('class', 'editedMember');
  newForm.setAttribute('id', id);
  member.appendChild(newForm);
  document.getElementsByTagName('button')[3].addEventListener('click', function(event){
    event.preventDefault();
    var form = document.getElementsByClassName('editedMember')[0];
    document.getElementById('family_members').children[0].innerHTML = null;
    removeFamilyMember(form.id);
    updateFamilySection(event, form);
    document.getElementsByClassName('editedMember')[0].style.display = 'none';
  });
}

function hideAttributes(attributes) {
  for (var i = 0; i < attributes.length; i++) {
    attributes[i].style.display = 'none';
  }
}
