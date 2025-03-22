# Assistente de Manutenção Preventiva

#### O Assistente de Manutenção Preventiva é uma ferramenta desenvolvida para auxiliar na manutenção e otimização do Windows, permitindo realizar diversas tarefas automáticas, como limpeza de arquivos temporários, desativação de processos e backup de configurações importantes.

#

![Screenshot_6](https://github.com/user-attachments/assets/2ca9a1de-2035-486b-8b0b-0839a8a295cc)


#

### Funcionalidades
- [x] Notificação UAC ao Usuário
- [x] Limpar tarefas Agendadas
- [x] Limpar Lixeira
- [x] Desativar Processos
- [x] Limpar Pasta %Temp%
- [x] Desativar o Windows Update
- [x] Limpar Google Update
- [x] Backup Registro
- [x] Pagefile.sys
- [x] Backup de Driver
- [x] Limpar Registros
- [x] Usuário "Suporte"
- [x] Limpar Prefetch
- [x] Backup do BootBCD 
- [x] Salva Log dos processos executados 

### Requisitos
- Windows 10 ou superior
- Permissões de administrador para executar

### Clone este repositório:
``` bash
git clone https://github.com/DanielCampos2017/MeuSuporte.git
```

### Contribuição
#### Sinta-se à vontade para sugerir melhorias ou relatar problemas abrindo uma issue no repositório.
# 

### Documentação das funcionalidade do programa




<details>
 <summary>🔽 Notificação UAC ao Usuário </summary>
 
 ``` bash
Ativa\Desativa Controle de Conta de Usuário (UAC) para Usuário.
Garantindo que as ações do programa sejam executadas com permissões elevadas.
 ```
</details>


<details>
 <summary>🔽 Limpar tarefas Agendadas </summary>
 
 ``` bash
Remove tarefas agendadas desnecessárias que podem estar consumindo recursos ou afetando o desempenho do sistema.
 ```
</details>

<details>
 <summary>🔽 Limpar Lixeira </summary>
 
 ``` bash
Exclui permanentemente os arquivos armazenados na Lixeira do usuário atual para liberar espaço em disco.
 ```
</details>

<details>
  <summary>🔽 Desativar Processos</summary>
 
``` bash
Finaliza de Remove os processos específicos em segundo plano que podem estar consumindo recursos sem necessidade.

[Serviços]
● AdobeUpdateService #Serviço da Adobe
● AGSService #Serviço da Adobe
● AdobeARMservice #Serviço da Adobe
● DbxSvc #Dropbox
● Microsoft Office Groove Audit Service #Serviço do Office
● SysMain #Pode ajudar em SSDs, mas em HDs pode ser útil
● UsoSvc #Atualização do Serviço Orchestrator
● XboxGipSvc #Xbox Accessory Management Service
● vmicvmsession #Serviço Direto do Hyper-V PowerShell
● vmicrdv #Serviço de Virtualização de Área de Trabalho Remota do Hyper-V
● UevAgentService #User Experience Virtualization
● vmickvpexchange #Serviço de Troca de Dados do Hyper-V
● PhoneSvc #Serviço de Telefonia
● vmictimesync #Serviço de Sincronização de Data/Hora do Hyper-V
● perceptionsimulation #Serviço de Simulação de Percepção do Windows
● SensorService #Serviço de Sensor rotação automática
● XboxNetApiSvc #Serviço de Rede Xbox Live
● vmicheartbeat #Serviço de Pulsação do Hyper-V
● HvHost #Serviço de Host HV
● cloudidsvc #Serviço de identidade Microsoft Cloud
● icssvc #Serviço de Hotspot Móvel do Windows
● fhsvc #Serviço de Histórico de Arquivos
● lfsvc #Serviço de Geolocalização
● refsdedupsvc #Serviço de duplicação do ReFS
● vmicshutdown #Serviço de Desligamento de Convidado do Hyper-V
● WMPNetworkSvc #Serviço de Compartilhamento de Rede do Windows Media Player
● WbioSrvc #Serviço de Biometria do Windows
● Fax #Se você não usa fax, pode desativar
● SEMgrSvc #Serviço de Gerenciador de NFC/SE e Pagamentos
● MapsBroker #Serviço de Mapas
● WpnService #Serviço de Notificações Push do Windows
● XblAuthManager #Serviço de Autenticação Xbox Live
 ```
</details>

<details>
 <summary>🔽 Limpar Pasta %Temp% </summary>
 
 ``` bash
Remove arquivos temporários do usuário e do sistema para liberar espaço e melhorar o desempenho.

[Arquivos]
● C:\Users\"Seu Usuario"\AppData\Local\Temp
 ```
</details>

<details>
  <summary>🔽 Desativar o Windows Update</summary>

 ``` bash
Desativa o serviço do Windows Update para evitar atualizações automáticas que podem impactar o desempenho ou causar reinicializações indesejadas.

[Arquivos]
● C:\Windows\SoftwareDistribution\Download\

[Serviços]
● InstallService #Serviço de Instalação da Microsoft Store
● svsvc #Serviço de Verificador de Ponto
● wuauserv #Serviço de Windows Update
● WSearch #Serviço de Pesquisa do Windows
 ``` 
</details>

<details>
 <summary>🔽 Limpar Google Update</summary>
 
``` bash
Remove serviços e processos relacionados ao Google Update, evitando consumo excessivo de recursos por atualizações automáticas.

[Arquivos]
● C:\Program Files (x86)\Google\Update\

[Serviços]
● edgeupdate #Serviço do google
● edgeupdatem #Serviço do google
● gupdatem  #Serviço do google
● gupdate  #Serviço do google
● GoogleChromeElevationService  #Serviço do google
● gusvc  #Serviço do google
```
</details>

<details>
  <summary>🔽 Backup Registro</summary>

 ``` bash
Faz uma cópia de segurança do Registro do Windows antes de aplicar otimizações, garantindo a possibilidade de restauração em caso de problemas.

[Registros]
● Computador\HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run
● Computador\HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Run
● Computador\HKEY_LOCAL_MACHINE\Software\WOW6432Node\Microsoft\Windows\CurrentVersion\Run 

 ``` 
</details>

<details>
  <summary>🔽 Otimizar Memória Virtual</summary>

 ``` bash
Otimiza o arquivo de paginação do Windows (Pagefile.sys), ajustando seu tamanho ou desativando-o conforme a necessidade do usuário.
 ``` 
</details>

<details>
 <summary>🔽 Backup de Drivers </summary>
 
 ``` bash
Cria uma cópia de segurança dos drivers instalados no sistema, permitindo restaurá-los em caso de falha ou reinstalação do Windows.
Use o comando para restaurar o Backup
[cmd] - administrador
 pnputil /add-driver "C:\DriversBackup\*.inf" /subdirs /install
 ```
</details>

<details>
 <summary>🔽 Limpar Registros</summary>
 
``` bash
Remove entradas desnecessárias ou obsoletas do Registro do Windows para melhorar a estabilidade e o desempenho.

[Registros]
● Computador\HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run
● Computador\HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Run
● Computador\HKEY_LOCAL_MACHINE\Software\WOW6432Node\Microsoft\Windows\CurrentVersion\Run
```
</details>

<details>

 <summary>🔽 Usuário "Suporte"</summary>
 
``` bash
Cria ou gerencia uma conta de usuário chamada "Suporte" para facilitar a manutenção e resolução de problemas no sistema.
```
</details>

<details>
 <summary>🔽 Limpar Prefetch</summary>
 
 ``` bash
Remover Arquivos de Inicialização Lenta (Prefetch)
Remove arquivos armazenados na pasta Prefetch localizada no diretório C:\Windows\Prefetch, que podem estar desatualizados e impactar negativamente o desempenho do sistema.
 ```
</details>

<details>
 <summary>🔽 Backup do BootBCD </summary>
 
 ``` bash
Faz uma cópia de segurança do Boot Configuration Data (BCD)
Para garantir que as configurações de inicialização possam ser restauradas caso ocorram problemas.
 ```
</details>

<details>
 <summary>🔽 Salva Log dos processos executados </summary>
 
 ``` bash
#   Ao tentar fechar o programa ele cria um log dos processos realizados. O log é salvo no mesmo diretorio de execução do programa
 ```
</details>
