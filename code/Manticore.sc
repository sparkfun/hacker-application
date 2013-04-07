
/*
* A master interface for pieces using OSCthulhu
*/
Manticore
{
	classvar <manticorePieces, <>group, <>users;

	classvar activePiece, osc;

	classvar win, <mainView, interface;

	classvar previousGUI, active;

	classvar usersWindow, chatWindow, scoreWindow, pieceListWindow;

	classvar <speakerConfiguration = \stereo, validSpeakerConfig;

	classvar <useSub = false;

	*initClass
	{
		manticorePieces = List.new;
		users = Dictionary.new;
		active = false;
		validSpeakerConfig = [ \stereo, \quad, \cube ];
	}

	*registerPiece
	{|className|
		manticorePieces.add(className);
	}

	*new
	{|speakerConfig, toUseSub|

		if(validSpeakerConfig.includes(speakerConfig),{
			speakerConfiguration = speakerConfig;
			},{
				speakerConfiguration = \stereo;
		});

		useSub = toUseSub;

		if(active == false,{
			this.init;
			active = true;
		});
	}

	*init
	{
		osc = List.new;
		this.setupOSClisteners;

		OSCthulhu.changePorts([57120]);
		OSCthulhu.login;

		activePiece = ManticorePiece.new;

		previousGUI = GUI.id;
		GUI.qt;
		QtGUI.palette = QPalette.dark;

		// setup recording for the type of speaker configuration we are using
		switch(speakerConfiguration,
			\stereo, { Server.default.recChannels = 2; },
			\quad, { Server.default.recChannels = 4; },
			\cube, { Server.default.recChannels = 8; }
		);

		this.makeWindow;
	}

	*free
	{
		activePiece.free;
		osc.collect({|def| def.permanent_(false); def.free; });
		GUI.fromID(previousGUI);
		active = false;
	}

	*makeWindow
	{
		var bottomPannel;
		var serverView, bootBtn, recordBtn;

		win = Window("Manticore",Window.screenBounds);
		win.acceptsMouseOver = true;
		win.front;
		win.onClose_({ this.free; });

		usersWindow = ManticoreUsersWindow();

		chatWindow = ManticoreChatWindow();
		scoreWindow = ManticoreScoreWindow();
		pieceListWindow = ManticorePieceListWindow(this);

		// server stuff
		bootBtn = Button();
		bootBtn.states_([
			["Boot",Color.white,Color.gray],
			["Quit",Color.white,Color.green(0.4)]
		]);
		bootBtn.action_({|btn|
			switch(btn.value,
				1,{ Server.default.boot; },
				0,{ Server.default.quit; }
			);
		});

		recordBtn = Button();
		recordBtn.states_([
			["record >",Color.white,Color.gray],
			["stop []",Color.white,Color.red(0.4)]
		]);
		recordBtn.action_({|btn|
			switch(btn.value,
				1,{
					Server.default.prepareForRecord;
					Server.default.record;
				},
				0,{ Server.default.stopRecording; }
			);
		});

		if(Server.default.pid.notNil,{ { bootBtn.value_(1); }.defer; });

		bottomPannel = View().layout_(
			HLayout(
				[chatWindow.getView, stretch: 3],
				[scoreWindow.getView, stretch: 3],
				[pieceListWindow.getView, stretch: 1]
			)
		);
// ^StackLayout( view, TextView().string_("hello world")).mode_(\stackAll);
		win.layout_(
			VLayout(
				mainView = [View().minHeight_(250), stretch:24],
				HLayout(
					usersWindow.getView,
					StaticText().string_("Server:"),
					bootBtn,
					recordBtn
				),
				[bottomPannel, stretch:1]
			)
		);

		View.globalKeyDownAction_({|view, char, modifiers, unicode, keycode, key|
			if(char == $t,{
				chatWindow.chatEntry.focus;
			});
		});

		mainView = mainView[0];

		interface = activePiece.getInterface(mainView);

		OSCthulhu.getChat;
	}

	*setupOSClisteners
	{
		osc.add(
			OSCthulhu.onUserName(\setOSCthulhuUserName,{|msg|
				/*				OSCthulhu.userName = msg[1];
				("OSCthulhu Username:" + OSCthulhu.userName).postln;*/
			});
		);

		osc.add(OSCthulhu.onChat(\receiveChat, {|msg|
			{
				chatWindow.chatDisplay.string = chatWindow.chatDisplay.string ++ msg[1] ++ "\n";
				// hack to enable autoscroll
				chatWindow.chatDisplay.select(chatWindow.chatDisplay.string.size-2,0);
			}.defer;
			});
		);

		osc.add(OSCthulhu.onAddPeer(\addingPeer, {|msg|
			users.put(msg[1], ManticoreUser.new(msg[1]));
			usersWindow.update;
			});
		);

		osc.add(OSCthulhu.onRemovePeer(\removingPeer, {|msg|
			users.removeAt(msg[1]);
			usersWindow.update;
			});
		);

		osc.add(OSCthulhu.onGetChat(\getChat, {|msg|
			{
				chatWindow.chatDisplay.string = msg[1];
				// hack to enable autoscroll
				chatWindow.chatDisplay.select(chatWindow.chatDisplay.string.size-2,0);
			}.defer;
			});
		);

		osc.collect({|def| def.permanent_(true); });
	}

	*changePiece
	{|className|
		activePiece.free;
		mainView.removeAll;
		activePiece = className.asClass.new(this);
		interface = activePiece.getInterface(mainView);
		win.name = "Manticore :" + className;
	}

	*onScoreNext
	{|function|
		scoreWindow.nextBtn.action_(function);
	}

	*onScorePrevious
	{|function|
		scoreWindow.previousBtn.action_(function);
	}

	*updateScore
	{|score|
		{scoreWindow.scoreDisplay.string_(score)}.defer;
	}
}

/////////// Core View Classes  ///////////

ManticoreUsersWindow
{
	var view;

	*new
	{
		^super.new;
	}

	getView
	{
		view = View().layout_(HLayout());
		view.background_(Color.gray(0.2));

		this.update;

		^view;
	}

	update
	{
		{
			view.removeAll;
			view.layout_(HLayout());
			view.layout.add(StaticText().string_("Users:"));
			Manticore.users.do{|item|
				view.layout.add(StaticText().string_(item.name).stringColor_(item.color));
			};
			view.layout.add(nil);
		}.defer;
	}
}

/*
* Holds generic information used for displaying users
*/
ManticoreUser
{
	var <name, <color;

	*new
	{|n|
		^super.new.init(n);
	}

	init
	{|n|
		name = n;
		color = Color.rand;
	}
}

/*
* View for OSCthulhu chat
*/
ManticoreChatWindow
{
	var <chatDisplay, <chatEntry;

	*new
	{
		^super.new;
	}

	getView
	{
		var view;

		view = View();
		view.layout_(
			VLayout(
				StaticText().string_("Chat"),
				chatDisplay = [TextView(), stretch:1],
				chatEntry = [TextField(), stretch:1]
			);
		);

		chatDisplay = chatDisplay[0];
		chatEntry = chatEntry[0];

		chatDisplay.editable = false;
		chatDisplay.canFocus = false;
		chatDisplay.canReceiveDragHandler = {};

		chatEntry.action_({|field|
			OSCthulhu.chat(field.string);
			field.string = "";
		});

		^view;
	}


}

/*
* View for displaying performance instructions
*/
ManticoreScoreWindow
{
	var <scoreDisplay, <previousBtn, <nextBtn;

	*new
	{
		^super.new;
	}

	getView
	{
		var view, timer;

		timer = ManticoreTimer();

		view = View();
		view.layout_(
			VLayout(
				StaticText().string_("Score"),
				scoreDisplay = [TextView(), stretch:1],

				HLayout(
					previousBtn = [Button(), stretch:1],
					timer.getView,
					nextBtn = [Button(), stretch:1]
				)
			)
		);

		scoreDisplay = scoreDisplay[0];
		previousBtn = previousBtn[0];
		nextBtn = nextBtn[0];

		scoreDisplay.canFocus = false;
		scoreDisplay.canReceiveDragHandler = {};

		previousBtn.states_( [ ["<<"] ] );

		nextBtn.states_( [ [">>"] ] );

		^view;
	}

}

/*
* An h:m:s timer view
*/
ManticoreTimer
{
	var resetBtn, playBtn, timerDisplay;
	var timeRoutine, time=0;

	*new
	{
		^super.new.init;
	}

	init
	{
		this.makeTimeRoutine;
	}

	getView
	{
		var view;

		view = View();
		view.layout_(
			HLayout(
				resetBtn = [Button(), stretch:1],
				timerDisplay = [TextField(), stretch:1],
				playBtn = [Button(), stretch:1]
			);
		);

		resetBtn = resetBtn[0];
		timerDisplay = timerDisplay[0];
		playBtn = playBtn[0];

		timerDisplay.canFocus = false;
		timerDisplay.canReceiveDragHandler = {};
		timerDisplay.minWidth = 75;
		timerDisplay.align_(\center);
		timerDisplay.string_( time.asTimeString.asArray.copyRange(3,7) );

		resetBtn.states_( [ ["|<"] ] );
		resetBtn.action_({ this.reset; });

		playBtn.states_( [ [">"], ["||"] ] );
		playBtn.action_({|btn|
			switch(
				btn.value,
				0, { this.stop; },
				1, { this.start; }
			);
		});

		^view;
	}

	start {
		timeRoutine.play(AppClock);
	}

	stop {
		timeRoutine.stop;
		this.makeTimeRoutine;
	}
	reset {
		timeRoutine.stop;
		this.makeTimeRoutine;
		time = 0;
		timerDisplay.string_(0.asTimeString.asArray.copyRange(3,7));
		playBtn.value = 0;
	}

	makeTimeRoutine
	{
		timeRoutine = Routine({
			inf.do({
				|i|
				timerDisplay.string_(time.asTimeString.asArray.copyRange(3,7));
				1.wait;
				time = time+1;
			});
		});
	}

}

/*
* View for listing/loading all Manticore pieces
*/
ManticorePieceListWindow
{
	var pieceList, changeBtn, recBtn;

	*new
	{|parent|
		^super.new;
	}

	getView
	{
		var view;

		view = View();
		view.layout_(
			VLayout(
				StaticText().string_("Pieces"),
				pieceList = [ListView(), stretch:1],
				changeBtn = [Button(), stretch:1]//,
				//recBtn = [Button(), stretch:1]
			);
		);

		pieceList = pieceList[0];
		changeBtn = changeBtn[0];
		//recBtn = recBtn[0];

		pieceList.items = Manticore.manticorePieces.asArray;
		pieceList.canReceiveDragHandler = {};

		changeBtn.states_([ ["Load Piece"] ]);

		changeBtn.action_({|btn|
			this.loadPiece;
		});
		/*
		recBtn.states_([["Record"],["Stop Recording"]]);
		recBtn.action_({|b|
		switch(b.value,
		0, {
		Server.default.stopRecording;
		},
		1, {
		Server.default.prepareForRecord;
		{Server.default.record}.defer;
		}
		);
		});
		*/

		^view;
	}

	loadPiece
	{
		Manticore.changePiece(pieceList.item);
		OSCthulhu.chat("*** Joining" + pieceList.item + "***");
	}

}

/*
* Abstract base class for pieces using the Manticore system
*/
ManticorePiece
{

	*initClass
	{
		Class.initClassTree(Manticore);		// init Manticore first
	}

	// override with a method returning a View for the main interface of the piece
	getInterface
	{
		^UserView.new(Manticore.mainView, Rect(0,0,100,100));
	}

	free
	{

	}
}