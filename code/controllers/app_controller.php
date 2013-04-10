<?php
class AppController extends Controller {
	public $components = array('Session');
	public $helpers = array('Js');
	
	public $worker;
	
	function beforeAction() 
	{
		parent::beforeAction();
		$this->worker = $this->Session->read('worker');
		
		if (empty($this->worker))
		{
			//not logged in
			
			//don't redirect for main/login or main/logout
			if ($this->params['controller'] == 'main')
			{
				if ($this->params['action'] == 'logout' || $this->params['action'] == 'login')
				{
					return;
				}
			}
			
			$this->redirect('main/login');
			exit();
		}else{
			//we're logged in and hitting main/login just redirect to main
			if ($this->params['controller'] == 'main' && $this->params['action'] == 'login')
			{
				$this->redirect('main');
				exit();
			}
		}
	}
	
}

