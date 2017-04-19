/// *****************************
/// *       RadRotator          *
/// *****************************

function RadRotator(id)
{
	this.ID = id;
	this.Container = '';
	this.InnerContainer = '';
	this.CurrentFrame = 0;
	this.Frames = new Array();
	this.Tickers = new Array();	
	this.NextFrameDelay = 2500;
	this.FrameEffect = '';
	this.FrameChangeType = 'Frame';
	this.FrameHeight = 0;
	this.FrameWidth = 0;
	this.ScrollSpeed = 30;
	this.ScrollHeight = 0;
	this.ScrollWidth = 0;
	this.ScrollDirection = 'vertical';
	this.Paused = false;
	this.Browser = "uplevel";
}

RadRotator.prototype.ClearTicker = function()
{
	var firstIndex;
	var lastIndex;
	
	if (this.Tickers.length)
	{		
		tlrkScrollReadyState = false;
		var tickerCounter = this.Tickers.length;
		firstIndex = (this.CurrentFrame) * (tickerCounter / this.Frames.length);
		lastIndex = firstIndex + (tickerCounter / this.Frames.length);
	}
	
	for (var i=firstIndex; i<lastIndex; i++)
	{
		var ticker = this.Tickers[i];
		var con = document.getElementById(ticker.Container);
		con.innerHTML = '';	
	}
}

RadRotator.prototype.IncrementFrame = function()
{
	if (this.CurrentFrame < this.Frames.length - 1)
		this.CurrentFrame++;
	else
		this.CurrentFrame = 0;
}

RadRotator.prototype.ShowFrame = function()
{
	var innerContainer = document.getElementById(this.InnerContainer);
	var Container = document.getElementById(this.Container);
	var currentFrame = document.getElementById(this.Frames[this.CurrentFrame]);
	
	innerContainer.style.visibility = 'hidden';
	innerContainer.innerHTML = currentFrame.innerHTML;
	
	if (this.FrameEffect && (this.Browser == "IE5" || this.Browser == "IE6"))
	{
		document.getElementById(this.InnerContainer).style.filter = this.FrameEffect;
		document.getElementById(this.InnerContainer).filters[0].apply();
	}

	innerContainer.style.visibility = 'visible';
	
	var instance = this;
	if (this.FrameEffect && (this.Browser == "IE5" || this.Browser == "IE6"))
	{
		document.getElementById(this.InnerContainer).filters[0].play();
		window.setTimeout( function() { instance.StartTickers(); }, 1200);		
	}
	else
	{
		this.StartTickers();
	}
}

RadRotator.prototype.StartTickers = function()
{
	if (this.Tickers.length)
	{		
		tlrkScrollReadyState = false;
		var tickerCounter = this.Tickers.length;
		firstTicker = this.Tickers[(this.CurrentFrame) * (tickerCounter / this.Frames.length)];
		firstTicker.Start();
	}
}


RadRotator.prototype.NextFrame = function()
{	
	this.IncrementFrame();
	this.ShowFrame();
	
	if (this.NextFrameDelay)
	{
		eval(this.ID + " = " + "this;");
		window.setTimeout( this.ID + ".NextFrame()", this.NextFrameDelay);
	}
}

// SCROLL

RadRotator.prototype.CopyNextToScrollBuffer = function()
{	
	var innerContainer = document.getElementById(this.InnerContainer);
	var parentTable = document.getElementById(this.InnerContainer).firstChild;
	var parentTBody = parentTable.getElementsByTagName("TBODY")[0];
	var parentRow = parentTable.getElementsByTagName("TR")[0];
	
	if (this.ScrollDirection == 'vertical')
	{	
		var clonedParentRow = parentRow.cloneNode(true);		
		parentTBody.removeChild(parentRow);		
		innerContainer.style.top = '0px';
		parentTBody.appendChild(clonedParentRow);		
	}
	else
	{		
		var parentCell = parentRow.getElementsByTagName("TD")[0];
		var clonedParentCell = parentCell.cloneNode(true);
		parentRow.removeChild(parentCell);
		innerContainer.style.left = '0px';	
		parentRow.appendChild(clonedParentCell);		
	}
	
	this.ClearTicker();
	this.IncrementFrame();		
		
	if (this.Tickers.length)
	{		
		tlrkScrollReadyState = false;
		var tickerCounter = this.Tickers.length;
		firstTicker = this.Tickers[(this.CurrentFrame) * (tickerCounter / this.Frames.length)];
		firstTicker.Start();
	}	
}

RadRotator.prototype.NextScroll = function()
{
	var innerContainer = document.getElementById(this.InnerContainer);	
	innerContainer.style.top = '0px';
	innerContainer.style.left = '0px';
	innerContainer.style.visibility = 'visible';
	
	if (this.Tickers.length)
	{		
		tlrkScrollReadyState = false;
		var tickerCounter = this.Tickers.length;
		firstTicker = this.Tickers[(this.CurrentFrame) * (tickerCounter / this.Frames.length)];
		firstTicker.Start();
	}		
	
	eval(this.ID + " = " + "this;");
	window.setTimeout( this.ID + ".ScrollFrames(false);", this.NextFrameDelay);
}


RadRotator.prototype.CalculateFrameSize = function()
{	
	this.FrameHeight = parseInt(document.getElementById(this.Container).style.height.replace('px',''));
	this.FrameWidth = parseInt(document.getElementById(this.Container).style.width.replace('px',''));	
	if (this.ScrollHeight == 0)
		this.ScrollHeight = this.FrameHeight;
	if (this.ScrollWidth == 0)
		this.ScrollWidth = this.FrameWidth;
}

RadRotator.prototype.InitScroll = function()
{
	if (this.ScrollDirection == 'vertical')
	{
		for (i=0; i<this.Frames.length; i++)
		{
			var cell = document.getElementById(this.InnerContainer + "_row" + i + "_cell");				
			var newHTML = document.getElementById(this.Frames[i]).innerHTML;		
			cell.innerHTML = newHTML;		
		}
	}
	else
	{
		for (i=0; i<this.Frames.length; i++)
		{
			var cell = document.getElementById(this.InnerContainer + "_row_cell" + i);
			var newHTML = document.getElementById(this.Frames[i]).innerHTML;		
			cell.innerHTML = newHTML;		
		}
	}
}

RadRotator.prototype.ScrollFrames = function(changeFrames)
{	
	var topx, leftx
	var changeFramesFlag = false;
	var changeFramesString = "false";
	var msTimeout = this.ScrollSpeed;
	var instance = this;
	
	if (this.Paused)
	{		
		eval(this.ID + " = " + "this;");
		changeFramesString = (changeFramesFlag) ? "true" : "false";
		window.setTimeout( this.ID + ".ScrollFrames(" + changeFramesString + ")" , msTimeout);
		return;
	}
	
	var innerContainer = document.getElementById(this.InnerContainer);
	if (this.ScrollDirection == 'vertical')
	{
		topx = parseInt(innerContainer.style.top.replace('px',''));
		topx = topx - 1;
		innerContainer.style.top = topx + 'px';	
	}
	else
	{
		leftx = parseInt(innerContainer.style.left.replace('px',''));
		leftx = leftx - 1;
		innerContainer.style.left = leftx + 'px';
	}
		
	if (this.ScrollDirection == 'vertical')
	{
		if (! (Math.abs(topx) % this.ScrollHeight))
		{
			msTimeout = this.NextFrameDelay;
			changeFramesFlag = true;
		}
		if (! (Math.abs(topx) % this.FrameHeight))
		{
			changeFramesFlag = true;
		}
	}
	else
	{
		if (! (Math.abs(leftx) % this.ScrollWidth))
		{
			msTimeout = this.NextFrameDelay;
			changeFramesFlag = true;
		}
		if (! (Math.abs(leftx) % this.FrameWidth))
		{			
			changeFramesFlag = true;
		}
	}
	
	if (changeFramesFlag)
	{		
		this.CopyNextToScrollBuffer();
	}
	
	changeFramesString = (changeFramesFlag) ? "true" : "false";
	eval(this.ID + " = " + "this;");
	window.setTimeout( this.ID + ".ScrollFrames(" + changeFramesString + ")" , msTimeout);
}

RadRotator.prototype.Start = function()
{
	if (this.FrameChangeType == "frame")
	{
		this.CurrentFrame = -1;
		this.NextFrame();
	}
	else
	{
		this.CalculateFrameSize();
		this.InitScroll();
		this.NextScroll();
	}
}

RadRotator.prototype.Pause = function()
{
	this.Paused = true;
}

RadRotator.prototype.UnPause = function()
{
	this.Paused = false;
}

/// *****************************
/// *       RadTicker           *
/// *****************************

function RadTicker(id)
{
	this.ID = id;
	this.Container = '';
	this.CurrentChar = 0;
	this.CurrentLine = 0;
	this.Lines = new Array();
	this.NewCharDelay = 50;
	this.NewLineDelay = 70;
	this.LineLoop = true;
	this.TimeoutID = 0;
	this.OnEnd = function () {};
}

RadTicker.prototype.Start = function()
{
	this.PrintNextChar(true);	
}

RadTicker.prototype.PrintNextChar = function(clearLine)
{
	var overflowFlag = false;
	var con = document.getElementById(this.Container);
	
	if (clearLine)
	{
		if (this.StatusBar)
			window.status = '';
		else
			con.innerHTML = '';
	}
	
	this.CurrentChar = this.CurrentChar + 1;
	
	if (this.CurrentChar > this.Lines[this.CurrentLine].length)
	{
		if (this.OnEnd)
		{
			this.OnEnd();
		}
		
		this.CurrentLine = this.CurrentLine + 1;
		this.CurrentChar = 0;
		if (this.CurrentLine > this.Lines.length - 1)
		{
			overflowFlag = true;
			this.CurrentLine = 0;
		}
		
		if (overflowFlag && this.LineLoop == false)
			return;
			
		eval(this.ID + " = " + "this;");
		window.setTimeout( this.ID + ".PrintNextChar(true)", this.NewLineDelay);
		return;
	}
	
	if (this.StatusBar)
		window.status += this.Lines[this.CurrentLine].charAt(this.CurrentChar-1);
	else
		con.innerHTML += this.Lines[this.CurrentLine].charAt(this.CurrentChar-1);
	
	eval(this.ID + " = " + "this;");
	window.setTimeout( this.ID + ".PrintNextChar(false)", this.NewCharDelay);
}