<?

class ClientController extends AppController {

	public $uses = array('Client', 'Casenote'); 
	public $helpers = array('form');

	function index()
	{
		$this->redirect('main');
	}
	
	function view($client_key) 
	{
		$this->set('worker', $this->worker);
		$client = $this->Client->FromKey($client_key);
		$this->set('client', $client);
	}

	function casenotes($client_key, $action = false, $note_key = false)
	{
		$this->set('worker', $this->worker);
		$client = $this->Client->FromKey($client_key);
		$this->set('client', $client);
		
		$error = false;

		//handle post actions, the view posts 'to itself' because everything works with and without ajax ... 
		if (isset($action) && !empty($this->params['form']))
		{
			
			$v = $this->validate('post', array(
				'date'=>'/^[0-9]{4}-[0-9]{2}-[0-9]{2}$/', //####-##-##
				'type'=>'/Phone|In Person|Email|Fax|Text/',
				'contactor'=>'/^[A-Za-z0-9_\-\',\+ :]{0,128}$/', //most chars, 128 limit
				'contactee'=>'/^[A-Za-z0-9_\-\',\+ :]{0,128}$/',
				'subject'=>'/^[A-Za-z0-9_\-\',\+ :]{0,256}$/', //most chars, 256 limit
				'comment'=>'ALLOW_EMPTY'
			));
			
			try
			{
				if ($v === true)
				{
					switch ($action)
					{
						case 'add':
							$note = $this->Casenote->insert($client_key, $this->worker['key'], $this->params['form']);
						break;
						case 'edit':
							$note = $this->Casenote->edit($note_key, $this->params['form']);
						break;
					}
				}
				else
				{
					$note = false;
				}
			}
			catch (ErrorException $e)
			{
				$error = $e->getMessage();
			}
			
			//ajax requests only update the row of information that has changed and the form html
			if ($this->params['isAjax'])
			{
				$this->set('note', $note);
				$this->render('casenote_ajax');
				return; 
			}
		}
		
		$this->set('error', $error);
		
		$casenotes = $this->Casenote->getListFromKey($client_key);
		$this->set('casenotes', $casenotes);
	}
	
	function search()
	{
		try
		{
			$client = $this->Client->searchByName($this->params['form']['search']);
		}
		catch(ErrorException $e)
		{
			$this->set('error', $e->getMessage());
			return;
		}
		
		$this->set('client', $client);
	}
	
	function casenote($key)
	{
		$this->layout = 'frame';
		$this->set('worker', $this->worker);
		
		$note = $this->Casenote->fromKey($key);
		$client = $this->Client->fromKey($note['client_key']);
		
		$this->set('note',$note);
		$this->set('client',$client);
	}

}