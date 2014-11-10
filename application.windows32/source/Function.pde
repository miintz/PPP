class Function
{  
  int pan1 = 0;
  
  Function () {
 
  }

  Properties loadCommandLine () {

    Properties props = new Properties();

    for (String arg : args) {
      String[] parsed = arg.split("=", 2);
      if (parsed.length == 2)
        props.setProperty(parsed[0], parsed[1]);
    }

    return props;
  }

  //tone
  float modeQuantize(float pitch, float[] mode, int keyOffset) {
    pitch = round(pitch / 2);    
    boolean inScale = false;    
    while (!inScale) 
    {
      for (int i=0; i<mode.length; i++) 
      {        
        if ((pitch - keyOffset) % 12 == mode[i]) 
          inScale = true;
        if (!inScale)
            pitch++;
      }
    }
    
    return pitch;
  }

  float getAngle(int hour, int minute)
  {   
    float diff = ((TransitHour * 60) + TransitMinute) - ((RiseHour * 60) + RiseMinute);
    float MinuteAngle = TransitAngle / diff; 

    if(Rising)
    {
      CurrentAngle = ((hour * 60) + minute) * MinuteAngle;
      RisingAngles.add(CurrentAngle);
    }
    else
    {
       //lol
       if(SetI != RisingAngles.size())
       {
         CurrentAngle = RisingAngles.get(SetI);
         SetI++;
       }
    }

    return CurrentAngle = modeQuantize(CurrentAngle, sc.MAJOR, keyOffset);
  }
  float pan2;
  int getPan()
  {             
    float PanDiff = 127.0 / trajectory.getSpeed();

    pan2 += PanDiff;

    if (pan2 >= 127.0)
      pan2 -= PanDiff;
    
    return int(pan2);
  }

  void readFile(String file)
  {    
    String[] lines =  loadStrings("data/"+file+".txt");   
    trajectory = new Trajectory(file, lines.length);
    
    for (int o = 0; o < lines.length; o++ )
    {    
      String line = lines[o];
      String[] words = split(line, ',');                           

      trajectory.setDate(words[0]);

      trajectory.setRiseAzDate(words[1]);
      trajectory.setRiseAzAngle(int(words[2]));

      trajectory.setTransitAltDate(words[3]);
      trajectory.setTransitAltAngle(int(words[4]));

      trajectory.setSetAzDate(words[5]);
      trajectory.setSetAzAngle(int(words[6]));

      trajectory.Index++;
    }    

    trajectory.Index = 0;     
  }
}

