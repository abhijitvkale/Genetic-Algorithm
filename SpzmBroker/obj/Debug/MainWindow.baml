   M S B A M L   `   `   `  ����E  ASpzmBroker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:FDM_GA_ProgramFDM_GA_Program  5   	       FDM_GA_Program.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:FDM_GA_Program     5   	     � Title$  
SpzmBroker��5	   	   $��627.5��6   $��627.5��6.   $��1032.216��6>   $��1032.216��6R    � Icon$ images/SpzmBroker.ico��6c   ���	#FF3C3C3C � 	Resources 5
         b2v��%   &	        �� 5   
   6'   5      .��5      ��  �� TabStripPlacement$
 Left=�6   $��Stretch=�6*   $��0q�6F   .@�5   
   �� ��5      �� 5       �� Orientation$ Vertical=�6   .+�5      �� $��40��6   $��40��6(   $��images/home.png��63   �� 5      $
��Home��6    5      .��5      � $��2,0,-2,0q�6   .+�5      �� -   5         ��Name$ 
txtDisplay��5      $��22(�6J   
���   �$��Segoe UI Light)�6k   $��Right=�6�   $��48��6�   $��0,523,355,0q�6�   $
��Wrap=�6�   $	��Top=�6�   $	��524��6�   ���	#FFFBFBFB �� TextAlignment$
 Left=�6   J� RenderTransformOrigin$ 0.471,-1.542&�6(  ��61   �� *	Results+6(  �� 5      -   5         ��Name$ lblDatabaseFolderPicker��5      $��	Database:��69   $��Right=�6M   $��Right=�6p   $��40��6�   $��
0,39,596,0q�6�   $	��Top=�6�   $	��100��6�   $��20(�6�   $��Segoe UI Light)�6�   $ 0.872,0.461&�6�   	 ^� ToolTip$f	 `Pick a database for AmiBroker to load. Make sure the Base Time Interval is correct in AmiBroker.��6  �� 5      -   5        $ lblDataSetFilePicker��5      $��Dataset:��66   $��Right=�6I   $��Right=�6l   $��40��6�   $��
0,84,596,0q�6�   $	��Top=�6�   $��90��6�   $��20(�6�   $��Segoe UI Light)�6�   $ 0.872,0.461&�6�   $@	 :Pick a dataset (.csv file) to load data into the database.��6  �� 5      -   5        $ lblFormatFilePicker��5      $��Format:��65   $��Right=�6G   $��Right=�6j   $��40��6�   $��0,129,596,0q�6�   $	��Top=�6�   $��90��6�   $��20(�6�   $��Segoe UI Light)�6�   $ 0.872,0.461&�6�   $�	 �Pick a format that is compatible with the dataset. If stored in the default 'Format' folder in AmiBroker, the entire path is not needed - just the name of the format (ex: dataoriginal.format).��6  �� 5      -   5        $ lblProgramFolderPicker��5      $��Program Folder:��68   $��Right=�6R   $��Right=�6u   $��40��6�   $��0,175,596,0q�6�   $	��Top=�6�   $	��146��6�   $��20(�6�   $��Segoe UI Light)�6�   $ 0.872,0.461&�6   $=	 7Pick the folder where SpzmBroker will save the results.��6$  �� 5      -   5        $ lblMaxRules��5      $��Maximum Rules:��6-   $��Segoe UI Light)�6F   $��20(�6b   $��Right=�6p   $��36��6�   $��0,232,118,0q�6�   $	��Top=�6�   $	��166��6�   $��Right=�6�   $_	 YMaximum number of rules that can be appended into one rule. (ex: X AND Y OR Z = 3 rules).��6�   �� 5      -   5        $ lblPopulationSize��5      $��Population Size:��63   $��Segoe UI Light)�6N   $��20(�6j   $��Right=�6x   $��40��6�   $��0,273,118,0q�6�   $	��Top=�6�   $	��166��6�   $��Right=�6�   $7	 1Number of chromosomes(AFL models) per generation.��6�   �� 5      -   5        $ lblGenerations��5      $��Number of Generations:��60   $��Segoe UI Light)�6Q   $��20(�6m   $��Right=�6{   $��40��6�   $��0,314,118,0q�6�   $	��Top=�6�   $	��220��6�   $��Right=�6�   $(	 "Number of generations to backtest.��6   �� 5      -	   5        $ lblProfitThresholdMinimum��5      $!��Save Results Higher Than $:��6;   $��Segoe UI Light)�6a   $��20(�6}   $��Right=�6�   $��40��6�   $��0,406,172,0q�6�   $	��Top=�6�   $	��247��6�   $��Right=�6�   $u	 oChromosomes that generate profits higher than this amount (in dollars) will be saved in 'SavedChromosomes.txt'.��6  �� 5      -
   5        $ lblStopSize��5      $��
Stop Size:��6-   $��Segoe UI Light)�6C   $��20(�6_   $
��Left=�6m   $��203,231,0,0q�6�   $	��Top=�6�   $	��220��6�   $��Right=�6�   $&	  Stop if loss of Number of points��6�   �� 5       -   5        $ 
lblPosSize��5       $��PositionSize��6,   $��Segoe UI Light)�6C   $��20(�6_   $
��Left=�6m   $��315,272,0,0q�6�   $	��Top=�6�   $ 0.019,0.459&�6�   �� 5"      -   5        
 ��Name$
 btnDatabseFolderPicker��5"      $	��...��69   $��Right=�6G   $��40��6c   $��	0,39,47,0q�6o   $	��Top=�6�   $��44��6�   $f	 `Pick a database for AmiBroker to load. Make sure the Base Time Interval is correct in AmiBroker.��6�   �� 5#      -   5        $
 btnDatasetFilePicker��5#      $	��...��67   $��Right=�6E   $��40��6a   $��	0,84,47,0q�6m   $	��Top=�6�   $��44��6�   $@	 :Pick a dataset (.csv file) to load data into the database.��6�   �� 5$      -   5        $
 btnFormatFilePicker��5$      $	��...��66   $��Right=�6D   $��40��6`   $��
0,129,47,0q�6l   $	��Top=�6�   $��44��6�   $8	 2Pick a format that is compatible with the dataset.��6�   �� 5%      -   5        $
 btnProgramFolderPicker��5%      $	��...��69   $��Right=�6G   $��40��6c   $��
0,174,47,0q�6o   $	��Top=�6�   $��44��6�   $=	 7Pick the folder where SpzmBroker will save the results.��6�   �� 5'      -   5         ��Name$ txtDatabaseFolderPicker��5'      $��Right=�6;   $��Center=�6W   $��40��6y   $��	0,39,96,0q�6�    �� TextWrapping$
 Wrap=�6�   $	��Top=�6�   $	��500��6�   $��15(�6�   $��Segoe UI Light)�6�   $f	 `Pick a database for AmiBroker to load. Make sure the Base Time Interval is correct in AmiBroker.��6,  �� 5(      -   5        $ txtDatasetFilePicker��5(      $��Right=�68   $��Center=�6T   $��40��6v   $��	0,84,96,0q�6�   $
 Wrap=�6�   $	��Top=�6�   $	��500��6�   $��15(�6�   $��Segoe UI Light)�6�   $@	 :Pick a dataset (.csv file) to load data into the database.��6&  �� 5)      -   5        $ txtFormatFilePicker��5)      $��Right=�67   $��Center=�6S   $��40��6u   $��
0,129,96,0q�6�   $
 Wrap=�6�   $	��Top=�6�   $	��500��6�   $��15(�6�   $��Segoe UI Light)�6�   $�	 �Pick a format that is compatible with the dataset. If stored in the default 'Format' folder in AmiBroker, the entire path is not needed - just the name of the format (ex: dataoriginal.format).��6%  �� 5*      -   5        $ txtProgramFolderPicker��5*      $��Right=�6:   $��Center=�6V   $��40��6x   $��
0,174,96,0q�6�   $
 Wrap=�6�   $	��Top=�6�   $	��500��6�   $��15(�6�   $��Segoe UI Light)�6�   $=	 7Pick the folder where SpzmBroker will save the results.��6+  �� 5+      -   5        $ txtMaxRules��5+      $��3��6/    �� 	MaxLength$ 4��6c   $��20(�6q   $��Right=�6   $��Center=�6�   $��36��6�   $��
0,232,47,0q�6�   $
 Wrap=�6�   $	��Top=�6�   $��66��6	   �� TextAlignment$ Center=�6   �� MaxLines$ 1��6+  $a	 [Maximum number of rules that can be appended into one rule. (ex: [X AND Y OR Z] = 3 rules).��68  �� 5,      -   5        $ txtPopulationSize��5,      $��10��65   $ 4��6j   $��20(�6x   $��Right=�6�   $��Center=�6�   $��36��6�   $��
0,273,47,0q�6�   $
 Wrap=�6�   $	��Top=�6�   $��66��6  $ Center=�6  $ 1��62  $7	 1Number of chromosomes(AFL models) per generation.��6?  �� 5-      -   5        $ txtGenerations��5-      $��10��62   $ 4��6g   $��20(�6u   $��Right=�6�   $��Center=�6�   $��40��6�   $��
0,314,47,0q�6�   $
 Wrap=�6�   $	��Top=�6�   $��66��6  $ Center=�6  $ 1��6/  $(	 "Number of generations to backtest.��6<  	� 5.      -   5         	�Name$ rbtnLong��5.      $��Long Trades��60    d� 	IsChecked$
 TrueY�6F   $��Right=�6W   $��0,367,166,0q�6s   $	��Top=�6�   $��16(�6�   $	��101��6�   $��34��6�   $��Segoe UI Light)�6�   $	 Generate long models.��6�   	� 5/      -   5        $ 	rbtnShort��5/      $��Short Trades��61   $��Right=�6H   $��
0,367,47,0q�6d   $	��Top=�6x   $ 
1.459,-1.4&�6�   $��16(�6�   $	��107��6�   $��34��6�   $��Segoe UI Light)�6�   $	 Generate long models.��6  �� 50      -   5        $ txtProfitFilterMinimum��50      $��1��6:   $ 10��6n   $��20(�6}   $��Right=�6�   $��Center=�6�   $��40��6�   $��
0,406,47,0q�6�   $
 Wrap=�6�   $	��Top=�6�   $	��120��6  $ Center=�6!  $ 1��68  $u	 oChromosomes that generate profits higher than this amount (in dollars) will be saved in 'SavedChromosomes.txt'.��6E  6�  �� 51      -   5        $
 	xbtnStart��51      $��Run GA��6,   $��Right=�6=   $��
0,485,19,0q�6Y   $	��Top=�6m   $	��148��6�   $��42��6�   $��Segoe UI Light)�6�   $��20(�6�   $ 	 Start the genetic program.��6�   �� 52      -   5        $
 xbtnStop��52      $
��Stop��6,   $
��Left=�6;   $��541,485,0,0q�6V   $	��Top=�6k   $	��141��6�   $��46��6�   6�   �� 53      -   5        $ txtStopSize��53      $��10��6/   $��36��6:   $��0,235,374,322q�6F   $ 4��6]   $��20(�6k   $��Right=�6y   $��Center=�6�   $
 Wrap=�6�   $��66��6�   �� 54      -   5        $ 
txtPosSize��54      $
��Left=�6.   $��36��6I   $ 4��6U   $��20(�6c   $��449,277,0,0q�6q   $
 Wrap=�6�   $	��Top=�6�   $��66��6�   (  "System.Windows.Controls.DatePicker  55      -   5          Name$ DtStartDate��55      $
��Left=�62   $��147,235,0,0q�6M   $	��Top=�6b     56      -   5        $ 	DtEndDate��56      $
��Left=�60   $��147,292,0,0q�6K   $	��Top=�6`   �� 57      $��Start Date:��6   $
��Left=�6.   $��
61,235,0,0q�6I   $	��Top=�6]   $ 0.146,1.593&�6u   $��81��6�   �� 58      $��	End Date:��6   $
��Left=�6,   $��
61,291,0,0q�6G   $	��Top=�6[   $��81��6s   �� 59      -    5        $ txtDisplay2��59      ���	#FFFBFBFB$��22(�6b   
���   �$��Segoe UI Light)�6�   $
��Left=�6�   $��	0,446,0,0q�6�   $
��Wrap=�6�   $	��Top=�6�   $��58��6�   $	��505��6  ��6H   �� *
Results2+6  �� 5:      -!   5         ��Name$ cmbIndicators��5:       �� Text$ Custom indicators��62   $
��Left=�6K   $��167,331,0,0q�6f   $	��Top=�6{   $	��164��6�   .@�5;      �� -"   5        $��kairi��5;      6/   �� 5<      -#   5        $��PercentRank��5<      65   �� 5=      -$   5        $��KalmanFilter��5=      66   �� 5>      -%   5        $��ITrend��5>      60   5@      �� 5A      $
��Left=�6   $��
10,409,0,0q�67   $
��Wrap=�6K   $	��Top=�6_   $��32��6w   $	��460��6�   �� 6�   $�	Best chro��6�   �� 6�   
����������	#FFA31515$��12.6666666666667(�6�   $��Consolas)�6�   .�6	  �� $(�"Best chromosome will be shown here��6  6:  �� 6@  $�mosome will be shown here��6D  6h  �� 5B      $��Custom Inicators��6   $
��Left=�63   $��
34,331,0,0q�6N   $	��Top=�6b   5G      5H      �� 5J   
   -&   5        $��tabGeneticSettings��5J      ��5K      �� 5L      $ Vertical=�6   .+�5M      �� $��40��6   $��40��6(   $��images/dna.png��63   �� 5N      $��Genetic Settings��6    5O      5Q      �� 5R   
   -'   5        $��tabAmiBrokerSettings��5R      ��5S      �� 5T      $ Vertical=�6   .+�5U      �� $��40��6   $��40��6(   $��images/gear.png��63   �� 5V      $��AmiBroker Settings��6    5W      5Y      �� 5Z   
   ��5[      �� 5\      $ Vertical=�6   .+�5]      �� $��40��6   $��40��6(   $��images/help.png��63   �� 5^      $��Information��6    5_      .��5a      � .+�5b      �� -(   5         ��Name$ image��5b      $!��images/SpzmBroker_named.png��6'   $��Right=�6L   $	��317��6h   $��-18,236,511,0q�6u   $	��Top=�6�   $	��322��6�   $	 SpzmBroker 2016.��6�   $ 0.451,0.309&�6�   5c      5d      5e      5i      