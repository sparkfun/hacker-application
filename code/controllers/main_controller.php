<?

class MainController extends AppController {

	public $uses = array('Worker', 'Caseload'); 

	function index() {		
		$this->set('caseload', $this->Caseload->getListFromWorkerKey($this->worker['key']));
	}
	
	function login() {
		$this->layout = 'no_menu';
		if (isset($this->params['form']['user']) && isset($this->params['form']['pass']))
		{
			try
			{
				$worker = $this->Worker->fromNickAndPass($this->params['form']['user'], $this->params['form']['pass']);
				$this->Session->write('worker', $worker);
				$this->redirect('main');
			}
			catch(ErrorException $e)
			{
				$this->set('error', $e->getMessage());
			}
		}

	}
	
	function add()
	{
		try
		{
			$this->Caseload->addClient($this->worker['key'], $this->params['form']['client_key']);
		}
		catch(ErrorException $e)
		{
			$this->set('add_error', $e->getMessage());
		}
		$this->set('caseload', $this->Caseload->getListFromWorkerKey($this->worker['key']));
		$this->render('index');
	}
	
	function remove()
	{
		try
		{
			$this->Caseload->removeClient($this->worker['key'], $this->params['form']['client_key']);
		}
		catch(ErrorException $e)
		{
			$this->set('edit_error', $e->getMessage());
		}
	}
	
	function logout() 
	{
		$this->layout = 'no_menu';
		$this->Session->destroy();
		$this->render('login');
	}

}