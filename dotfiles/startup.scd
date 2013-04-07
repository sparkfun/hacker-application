//~sccode = OSCresponderNode(nil,'/SCCode',{|t,r,msg| msg[1].asString.interpret;}).add;

~options = Server.local.options;
~options.memSize_(4096*10);
~options.blockSize_(64);

~options.numInputBusChannels = 40;
~options.numOutputBusChannels = 42;

s.recSampleFormat = "float";

AutoBackup.start;
/*
AutoCompMethodBrowser.exclude([\if, \do, \while, \loop, \collect, \select, \reject, \detect, \add, \put, \at]);

Document.globalKeyDownAction = Document.globalKeyDownAction.addFunc(
{
	arg doc, char, mod, unicode, keycode;
	//doc.postln; char.ascii.postln; mod.postln; unicode.postln; keycode.postln;
	if(doc.name.find("html").isNil and: (doc.isListener == false),
	{
	doc.autoComplete;
		if(unicode==13 or:(unicode==32) or: (unicode==3) or: (unicode==46),
		{�
			doc.syntaxColorize;
		});

		if(unicode==17,	// control-q
		{
			Quarks.gui;
		});
	});
});

"\n *** press control-q for the Quarks gui *** \n".postln;

// color the post window
Document.listener.background = Color(0, 0, 0.0, 0.9);
Document.listener.stringColor=Color(0,0.9,0);
Document.postColor_(Color(0,0.9,0));
*/

// make a global var for the default_group
~default_group = Group.basicNew(s, 1); // the default group of s

Server.default.makeGui;

//~options.maxLocalBufs = 40;