import os
import shutil
from pathlib import Path

def copy_directory(source_dir: str, destination_dir: str):
    source_path = Path(source_dir)
    destination_path = Path(destination_dir)
        
    destination_path.mkdir(parents=True, exist_ok=True)
        
    for item in source_path.iterdir():
        destination_filepath = destination_path / item.name
        if item.is_dir():
            if destination_filepath.exists():
                shutil.rmtree(destination_filepath)
            shutil.copytree(item, destination_filepath)
        else:
            shutil.copy2(item, destination_filepath)
            
    print(f"Directory Copied: {dir_to_copy}")

if __name__ == "__main__":  
    dir_to_copy = "UrsurConfigs"      
    mod_dir = "Ursur"

    content_dir = mod_dir

    
    script_dir = os.path.dirname(os.path.abspath(__file__))
    source = f"{script_dir}/{dir_to_copy}"
    destination = f"../../config/Obeliskial_importing/{content_dir}"
    
    copy_directory(source, destination)

    mod_destination = f"{script_dir}/{mod_dir}/BepInEx/config/Obeliskial_importing/{content_dir}"
    copy_directory(source, mod_destination)

    output_name = mod_dir
    zip_dir = mod_dir
    shutil.make_archive(output_name, 'zip', zip_dir)
